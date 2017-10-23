using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class CreateSoldierCommand
    {
        public CreateSoldierCommand(string session)
        {
            Session = session;
        }

        public string Session { get; set; }
    }
}