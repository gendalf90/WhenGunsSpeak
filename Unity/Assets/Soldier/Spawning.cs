using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Soldier
{
    public class Spawning : MonoBehaviour
    {
        public void Spawn(Vector2 posiiton)
        {
            GetComponent<SoldierPositionEventSender>().enabled = true;
            GetComponent<Mouse>().enabled = true;
            GetComponent<Keyboard>().enabled = true;
        }
    }
}
