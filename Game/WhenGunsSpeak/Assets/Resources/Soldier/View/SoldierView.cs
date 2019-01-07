using Camera;
using Messages;
using System;
using UnityEngine;

namespace Soldier
{
    class SoldierView : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<PositionEvent>(HandleSoldierPositionEvent);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<PositionEvent>(HandleSoldierPositionEvent);
        }

        private void HandleSoldierPositionEvent(PositionEvent e)
        {
            if (PlayerGuid == e.PlayerGuid)
            {
                observable.Publish(new SetCameraPositionCommand(e.Position));
            }
        }

        public Guid PlayerGuid { get; set; }
    }
}
