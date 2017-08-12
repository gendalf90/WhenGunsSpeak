using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Server
{
    class Registration : IPacket
    {
        public Registration(Guid guid, string description)
        {
            Guid = guid;
            Description = description;
        }

        public Guid Guid { get; private set; }

        public string Description { get; private set; }

        public byte[] GetBytes()
        {
            var registration = new { Action = "registration", From = Guid.ToMinString(), Description = Description };
            return new BinaryDataBuilder().WriteAsJson(registration).Build();
        }
    }
}
