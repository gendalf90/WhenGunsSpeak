using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Soldier.Motion
{
    class Position : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SetSoldierPositionCommand>(Change);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SetSoldierPositionCommand>(Change);
        }

        private void Change(SetSoldierPositionCommand command)
        {
            if(Session != command.Session)
            {
                return;
            }
            
            transform.position = command.Position;
        }

        private void Update()
        {
            observable.Publish(new PositionEvent(Session, transform.position));
        }

        public string Session { get; set; }
    }
}
