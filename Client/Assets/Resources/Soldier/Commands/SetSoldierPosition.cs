﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    public class SetSoldierPositionCommand
    {
        public SetSoldierPositionCommand(string session, Vector2 position)
        {
            Session = session;
            Position = position;
        }

        public string Session { get; private set; }

        public Vector2 Position { get; private set; }
    }
}