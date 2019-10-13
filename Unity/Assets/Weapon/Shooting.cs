using UnityEngine;

namespace Weapon
{
    public class Shooting : MonoBehaviour
    {
        public void StartShooting()
        {
            foreach (var shootable in GetComponentsInChildren<IShootable>())
            {
                shootable.StartShooting();
            }
        }

        public void StopShooting()
        {
            foreach (var shootable in GetComponentsInChildren<IShootable>())
            {
                shootable.StopShooting();
            }
        }
    }
}
