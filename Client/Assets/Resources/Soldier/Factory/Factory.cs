using Messages;
using Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier.Factory
{
    public class Factory : MonoBehaviour
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
            observable.Subscribe<OnStartedAsClientEvent>(OnStart);
            observable.Subscribe<OnStartedAsServerEvent>(OnStart);
            observable.Subscribe<OnStoppedEvent>(OnStop);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<OnStartedAsClientEvent>(OnStart);
            observable.Unsubscribe<OnStartedAsServerEvent>(OnStart);
            observable.Unsubscribe<OnStoppedEvent>(OnStop);
        }

        private void OnStart(OnStartedAsClientEvent e)
        {
            creator.IsClient = true;
            creator.enabled = true;
            creator.Session = e.MySession;
        }

        private void OnStart(OnStartedAsServerEvent e)
        {
            creator.IsServer = true;
            creator.enabled = true;
            creator.Session = e.MySession;
        }

        private void OnStop(OnStoppedEvent e)
        {
            creator.IsServer = false;
            creator.IsClient = false;
            creator.enabled = false;
        }
    }
}