using UnityEngine;

namespace Weapon
{
    public class Shooting : MonoBehaviour
    {
        private IShootable[] shootables;

        private void Awake()
        {
            shootables = GetComponentsInChildren<IShootable>();
        }

        public void StartShooting()
        {
            foreach (var shootable in shootables)
            {
                shootable.StartShooting();
            }
        }

        public void StopShooting()
        {
            foreach (var shootable in shootables)
            {
                shootable.StopShooting();
            }
        }
    }
}
