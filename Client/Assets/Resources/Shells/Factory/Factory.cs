using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Messages;

namespace Shells.Factory
{
    class Factory : MonoBehaviour
    {
        private Observable observable;
        private Creator creator;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
            creator = GetComponent<Creator>();
        }

        private void OnEnable()
        {
            observable.Subscribe<OnStartedAsClientEvent>(StartAsClient);
            observable.Subscribe<OnStartedAsServerEvent>(StartAsServer);
            observable.Subscribe<OnStoppedEvent>(OnStop);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnStartedAsClientEvent>(StartAsClient);
            observable.Unsubscribe<OnStartedAsServerEvent>(StartAsServer);
            observable.Unsubscribe<OnStoppedEvent>(OnStop);
        }

        private void StartAsClient(OnStartedAsClientEvent e)
        {
            creator.enabled = true;
            creator.IsServer = false;
            creator.IsClient = true;
        }

        private void StartAsServer(OnStartedAsServerEvent e)
        {
            creator.enabled = true;
            creator.IsServer = true;
            creator.IsClient = false;
        }

        private void OnStop(OnStoppedEvent e)
        {
            creator.enabled = false;
            creator.IsServer = false;
            creator.IsClient = false;
        }
    }
}