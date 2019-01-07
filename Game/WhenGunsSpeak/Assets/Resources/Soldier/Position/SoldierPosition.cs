using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class SoldierPosition : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<SetSoldierPositionCommand>(HandleSoldierPositionCommand);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<SetSoldierPositionCommand>(HandleSoldierPositionCommand);
        }

        private void HandleSoldierPositionCommand(SetSoldierPositionCommand command)
        {
            if(PlayerGuid == command.PlayerGuid)
            {
                Position = command.Position;
            }
        }

        public Guid PlayerGuid { get; set; }

        private void Update()
        {
            observable.Publish(new PositionEvent(PlayerGuid, Position));
        }

        private Vector2 Position
        {
            set
            {
                transform.position = value;
            }
            get
            {
                return transform.position;
            }
        }
    }
}
