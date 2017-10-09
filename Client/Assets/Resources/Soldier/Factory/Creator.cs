using Messages;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeadBehaviour = Soldier.Head.Head;
using BodyBehaviour = Soldier.Body.Body;
using LegsBehaviour = Soldier.Legs.Legs;
using Soldier.Weapon;

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

        private Guid guid;
        private GameObject result;

        private Dictionary<Guid, GameObject> instantiated = new Dictionary<Guid, GameObject>();

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

        private void Create(CreateSoldierCommand command)
        {
            InitializeCommand(command);
            CreateSoldier();
            BindHead();
            BindBody();
            BindLegs();
            BindWeapon();
        }

        private void InitializeCommand(CreateSoldierCommand command)
        {
            guid = command.Guid;
        }

        private void CreateSoldier()
        {
            result = Instantiate(soldier);
            var rigidbody = result.GetComponent<Rigidbody2D>();
            rigidbody.isKinematic = IsClient;
            instantiated.Add(guid, result);
        }

        private void BindHead()
        {
            var handle = result.transform.Find("HeadHandle");
            var instance = Instantiate(head);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<HeadBehaviour>();
            behaviour.Guid = guid;
        }

        private void BindBody()
        {
            var handle = result.transform.Find("BodyHandle");
            var instance = Instantiate(body);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<BodyBehaviour>();
            behaviour.Guid = guid;
        }

        private void BindLegs()
        {
            var handle = result.transform.Find("LegsHandle");
            var instance = Instantiate(legs);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<LegsBehaviour>();
            behaviour.Guid = guid;
        }

        private void BindWeapon()
        {
            var handle = result.transform.FindChild("Hands");
            var instance = Instantiate(weapon);
            instance.transform.SetParent(handle);
            var behaviour = instance.GetComponent<AKM>();
            behaviour.Guid = guid;
        }

        private void Remove(RemoveSoldierCommand command)
        {
            GameObject instance;

            if(!instantiated.TryGetValue(command.Guid, out instance))
            {
                return;
            }

            instantiated.Remove(command.Guid);
            Destroy(instance);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<CreateSoldierCommand>(Create);
            observable.Unsubscribe<RemoveSoldierCommand>(Remove);
        }
    }
}