using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class RegisterRoomCommand
    {
        public RegisterRoomCommand(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}
