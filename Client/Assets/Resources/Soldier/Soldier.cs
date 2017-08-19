using Messages;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Soldier
{
    class Soldier : MonoBehaviour
    {
        private Observable observable;

        private void Awake()
        {
            observable = FindObjectOfType<Observable>();
        }


    }
}
