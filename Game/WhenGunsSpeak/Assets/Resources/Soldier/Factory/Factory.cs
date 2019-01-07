using Messages;
using Server;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class Factory : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }

        private void OnEnable()
        {
            observable.Subscribe<CreateRoomPlayerSoldierCommand>(HandleCreateRoomPlayerSoldierCommand);
        }

        private void OnDisable()
        {
            observable.Unsubscribe<CreateRoomPlayerSoldierCommand>(HandleCreateRoomPlayerSoldierCommand);
        }

        private void HandleCreateRoomPlayerSoldierCommand(CreateRoomPlayerSoldierCommand command)
        {

        }
    }
}