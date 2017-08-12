using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    public class RegisterCommand
    {
        public RegisterCommand(string description)
        {
            Description = description;
        }

        public string Description { get; private set; }
    }
}
