using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Shells
{
    class Shell : MonoBehaviour
    {
        public Guid Guid { get; set; }

        public string Type { get; set; }

        public Vector2 Position
        {
            get
            {
                return transform.position;
            }
        }

        public float Rotation
        {
            get
            {
                return transform.rotation.z;
            }
        }
    }
}
