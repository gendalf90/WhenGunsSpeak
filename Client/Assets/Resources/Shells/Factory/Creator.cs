using Messages;
using Server;
using Shells.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ShellCollision = Shells.Physics.Collision;

namespace Shells.Factory
{
    class Creator : MonoBehaviour
    {
        [SerializeField]
        private int pullCapacity;

        private Observable observable;

        private GameObject prefab;
        private Dictionary<Guid, GameObject> activated;
        private Queue<GameObject> pulled;

        private string type;
        private Guid guid;
        private Vector2 position;
        private float rotation;
        private float force;
        private GameObject current;

        public Creator()
        {
            pulled = new Queue<GameObject>();
            activated = new Dictionary<Guid, GameObject>();
        }

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Shells/Shell");
            observable = FindObjectOfType<Observable>();
        }

        private void Start()
        {
            FillPull();
        }

        private void FillPull()
        {
            for (int i = 0; i < pullCapacity; i++)
            {
                CreateAndPull();
            }
        }

        private void CreateAndPull()
        {
            var newObject = Instantiate(prefab);
            newObject.SetActive(false);
            pulled.Enqueue(newObject);
        }

        private void OnEnable()
        {
            observable.Subscribe<Throw7dot62Command>(CreateAndThrowBullet);
            observable.Subscribe<CreateShellCommand>(CreateShell);
            observable.Subscribe<RemoveShellCommand>(RemoveShell);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<Throw7dot62Command>(CreateAndThrowBullet);
            observable.Unsubscribe<CreateShellCommand>(CreateShell);
            observable.Unsubscribe<RemoveShellCommand>(RemoveShell);
        }

        public bool IsServer { get; set; }

        public bool IsClient { get; set; }

        private void CreateAndThrowBullet(Throw7dot62Command command)
        {
            Initialize(command);
            Create();
            Throw();
        }

        private void Initialize(Throw7dot62Command command)
        {
            type = "7.62";
            guid = command.Guid;
            position = command.Position;
            rotation = command.Rotation;
            force = 2.0f;
        }

        private void CreateShell(CreateShellCommand command)
        {
            Initialize(command);
            Create();
        }

        private void Initialize(CreateShellCommand command)
        {
            guid = command.Guid;
            type = command.Type;
            position = command.Position;
            rotation = command.Rotation;
        }

        private void Create()
        {
            Load();
            SetPositionAndRotation();
            SetComponents();
            Activate();
        }

        private void Load()
        {
            if (pulled.Count == 0)
            {
                CreateAndPull();
            }

            current = pulled.Dequeue();
        }

        private void SetPositionAndRotation()
        {
            var transform = current.transform;
            transform.position = position;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        }

        private void SetComponents()
        {
            var shell = current.GetComponent<Shell>();
            shell.Guid = guid;
            shell.Type = type;
            var collision = current.GetComponent<ShellCollision>();
            collision.Guid = guid;
            var rigidbody = current.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = IsClient;
        }

        private void Activate()
        {
            current.SetActive(true);
            activated.Add(guid, current);
        }

        private void Throw()
        {
            var thrower = current.GetComponent<Throw>();
            thrower.WithForce(force);
        }

        private void RemoveShell(RemoveShellCommand command)
        {
            Initialize(command);
            Deactivate();
        }

        private void Initialize(RemoveShellCommand command)
        {
            guid = command.Guid;
        }

        private void Deactivate()
        {
            if (!activated.TryGetValue(guid, out current))
            {
                return;
            }

            current.SetActive(false);
            activated.Remove(guid);
            pulled.Enqueue(current);
        }

        private void Update()
        {
            NotifyAboutActivatedShells();
        }

        private void NotifyAboutActivatedShells()
        {
            var data = activated.Select(pair => pair.Value.GetComponent<Shell>())
                                .Select(component => new ShellData(component.Type, 
                                                                   component.Guid, 
                                                                   component.Position, 
                                                                   component.Rotation));

            observable.Publish(new ShellsEvent(data));
        }
    }
}
