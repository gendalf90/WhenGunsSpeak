using Messages;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeadBehaviour = Soldier.Head.Head;
using BodyBehaviour = Soldier.Body.Body;
using LegsBehaviour = Soldier.Legs.Legs;
using RotationBehaviour = Soldier.Rotation.Rotation;
using Soldier.Weapon;
using Soldier.Ground;
using LegsInput = Soldier.Legs.Input;
using MotionInput = Soldier.Motion.Input;
using RotationInput = Soldier.Rotation.Input;
using Soldier.Motion;
using Soldier.Network;

namespace Soldier.Factory
{
    public class Creator : MonoBehaviour
    {
        private Observable observable;
        private GameObject soldier;
        private GameObject head;
        private GameObject body;
        private GameObject legs;
        private GameObject weapon;

        private string session;
        private GameObject current;

        private Dictionary<string, GameObject> instantiated;

        public Creator()
        {
            instantiated = new Dictionary<string, GameObject>();
        }

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            soldier = Resources.Load<GameObject>("Soldier/SoldierPrefab");
            head = Resources.Load<GameObject>("Soldier/Head/HeadPrefab");
            body = Resources.Load<GameObject>("Soldier/Body/BodyPrefab");
            legs = Resources.Load<GameObject>("Soldier/Legs/LegsPrefab");
            weapon = Resources.Load<GameObject>("Soldier/Weapon/AKM/AKM");
        }

        private void OnEnable()
        {
            observable.Subscribe<CreateSoldierCommand>(Create);
            observable.Subscribe<RemoveSoldierCommand>(Remove);
        }

        public bool IsServer { get; set; }

        public bool IsClient { get; set; }

        public string Session { get; set; }

        private void Create(CreateSoldierCommand command)
        {
            InitializeCommand(command);
            CreateSoldier();
            BindHead();
            BindBody();
            BindLegs();
            SetGround();
            SetMotion();
            SetNetwork();
            SetRotation();
            //BindWeapon();
            NotifyThatCreated();
        }

        private void InitializeCommand(CreateSoldierCommand command)
        {
            session = command.Session;
        }

        private void CreateSoldier()
        {
            current = Instantiate(soldier);
            var rigidbody = current.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = IsClient;
            instantiated.Add(session, current);
        }

        private void BindHead()
        {
            var handle = current.transform.Find("HeadHandle");
            var instance = Instantiate(head);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<HeadBehaviour>();
            behaviour.Session = session;
        }

        private void BindBody()
        {
            var handle = current.transform.Find("BodyHandle");
            var instance = Instantiate(body);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<BodyBehaviour>();
            behaviour.Session = session;
        }

        private void SetGround()
        {
            var behaviour = current.GetComponentInChildren<Checker>();
            behaviour.Session = session;
            behaviour.enabled = IsServer;
        }

        private void BindLegs()
        {
            var handle = current.transform.Find("LegsHandle");
            var instance = Instantiate(legs);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<LegsBehaviour>();
            behaviour.Session = session;
            var input = instance.GetComponent<LegsInput>();
            input.enabled = IsPlayer;
        }

        private void SetMotion()
        {
            var movement = current.GetComponent<Movement>();
            movement.Session = session;
            var position = current.GetComponent<Position>();
            position.Session = session;
            var input = current.GetComponent<MotionInput>();
            input.enabled = IsPlayer;
        }

        private void SetNetwork()
        {
            var receiver = current.GetComponent<Receiver>();
            receiver.Session = session;
            var clientSender = current.GetComponent<ClientSender>();
            clientSender.Session = session;
            clientSender.enabled = IsClient;
            var serverSender = current.GetComponent<ServerSender>();
            serverSender.Session = session;
            serverSender.enabled = IsServer;
        }

        private void SetRotation()
        {
            var input = current.GetComponent<RotationInput>();
            input.enabled = IsPlayer;
            var behaviour = current.GetComponent<RotationBehaviour>();
            behaviour.Session = session;
        }

        //private void BindWeapon()
        //{
        //    var handle = result.transform.FindChild("Hands");
        //    var instance = Instantiate(weapon);
        //    instance.transform.SetParent(handle);
        //    var behaviour = instance.GetComponent<AKM>();
        //    behaviour.Guid = guid;
        //}

        private void NotifyThatCreated()
        {
            observable.Publish(new SoldierCreatedEvent(session));
        }

        private void Remove(RemoveSoldierCommand command)
        {
            GameObject instance;

            if(!instantiated.TryGetValue(command.Session, out instance))
            {
                return;
            }

            instantiated.Remove(command.Session);
            Destroy(instance);
            observable.Publish(new SoldierRemovedEvent(command.Session));
        }

        private bool IsPlayer
        {
            get
            {
                return Session == session;
            }
        }

        private void OnDisable()
        {
            observable.Unsubscribe<CreateSoldierCommand>(Create);
            observable.Unsubscribe<RemoveSoldierCommand>(Remove);
        }
    }
}