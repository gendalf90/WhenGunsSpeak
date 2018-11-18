using Messages;
using UnityEngine;

namespace Server
{
    class RoomOwnerState : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            SubscribeAll();
        }

        private void OnDisable()
        {
            UnsubscribeAll();
        }

        private void SubscribeAll()
        {
            
        }

        private void UnsubscribeAll()
        {
            
        }

        private void Disable()
        {
            enabled = false;
        }
    }
}
