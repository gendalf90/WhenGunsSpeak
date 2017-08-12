using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Messages;

namespace Shells
{
    class Factory : MonoBehaviour
    {
        [SerializeField]
        private int cacheCapacity;

        private GameObject prefab;
        private Observable observable;
        private Queue<GameObject> created;
        private Dictionary<Guid, GameObject> instantiated;

        private Guid currentGuid;
        private GameObject currentObject;

        public Factory()
        {
            created = new Queue<GameObject>();
            instantiated = new Dictionary<Guid, GameObject>();
        }

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Shells/Shell");
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            FillCreatedPull();
            observable.Subscribe<OnStartedEvent>(OnStart);
            observable.Subscribe<OnStoppedEvent>(OnStop);
        }

        private void FillCreatedPull()
        {
            for (int i = 0; i < cacheCapacity; i++)
            {
                CreateNewPrefab();
            }
        }

        private void CreateNewPrefab()
        {
            var newObject = Instantiate(prefab);
            newObject.SetActive(false);
            created.Enqueue(newObject);
        }

        private void OnStart(OnStartedEvent e)
        {
            IsServer = e.MyRole == Role.Server;
            IsClient = e.MyRole == Role.Client;

            if (IsClient)
            {
                observable.Subscribe<OnReceiveEvent>(Receive);
            }
        }

        private void OnStop(OnStoppedEvent e)
        {
            if (IsClient)
            {
                observable.Unsubscribe<OnReceiveEvent>(Receive);
            }

            IsServer = false;
            IsClient = false;
        }

        private bool IsServer { get; set; }

        private bool IsClient { get; set; }

        private void CreateAndThrowBullet(ThrowBulletCommand command)
        {
            SetGuid(command.Guid);
            Load();
            SetPosition(command.Position, command.Rotation);
            SetComponents();
            Instantiate();
            Throw();
        }

        private void SetGuid(Guid guid)
        {
            currentGuid = guid;
        }

        private void Load()
        {
            if (created.Count == 0)
            {
                CreateNewPrefab();
            }

            currentObject = created.Dequeue();
        }

        private void SetPosition(Vector2 position, float rotation)
        {
            var transform = currentObject.transform;
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        private void SetComponents()
        {
            var collision = currentObject.GetComponent<Collision>();
            collision.Guid = currentGuid;
            var client = currentObject.GetComponent<Client>();
            client.Guid = currentGuid;
            client.enabled = IsClient;
            var server = currentObject.GetComponent<Server>();
            server.Guid = currentGuid;
            server.enabled = IsServer;
            var rigidbody = currentObject.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = IsClient;
        }

        private void Instantiate()
        {
            currentObject.SetActive(true);
            instantiated.Add(currentGuid, currentObject);
        }

        private void OnHit(OnBulletHitEvent e)
        {
            SetGuid(e.Guid);
            Destroy();
        }

        private void Destroy()
        {
            if (!instantiated.TryGetValue(currentGuid, out currentObject))
            {
                return;
            }

            currentObject.SetActive(false);
            instantiated.Remove(currentGuid);
            created.Enqueue(currentObject);
        }

        private void Throw()
        {
            var thrower = currentObject.GetComponent<Throw>();
            thrower.Begin(2.0f);
        }

        private void Update()
        {
            if (!IsServer)
            {
                return;
            }

            var data = new FactoryData { InstantiatedGuids = instantiated.Keys.ToArray() };
            observable.Publish(new SendToClientsCommand(data.CreateBinaryObjectPacket()));
        }

        private void Receive(OnReceiveEvent e)
        {
            e.Data.OfBinaryObjects<FactoryData>()
                  .FirstOrDefault()
                  .Do(SynchronizeWithServerData);
        }

        private void SynchronizeWithServerData(FactoryData data)
        {
            var newGuids = data.InstantiatedGuids.Except(instantiated.Keys).ToList();
            newGuids.ForEach(ClientInstantiate);
            var oldGuids = instantiated.Keys.Except(data.InstantiatedGuids).ToList();
            oldGuids.ForEach(ClientDestroy);
        }

        private void ClientInstantiate(Guid guid)
        {
            SetGuid(guid);
            Load();
            SetComponents();
            Instantiate();
        }

        private void ClientDestroy(Guid guid)
        {
            SetGuid(guid);
            Destroy();
        }

        private void OnDestroy()
        {
            observable.Unsubscribe<OnStartedEvent>(OnStart);
            observable.Unsubscribe<OnStoppedEvent>(OnStop);
        }
    }
}