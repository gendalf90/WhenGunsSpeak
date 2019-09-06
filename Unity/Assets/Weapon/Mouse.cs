using UnityEngine;

namespace Weapon
{
    public class Mouse : MonoBehaviour
    {
        private Shooting shooting;

        private void Awake()
        {
            shooting = GetComponent<Shooting>();
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButtonDown(0))
            {
                shooting.StartShooting();
            }

            if (Input.GetMouseButtonUp(0))
            {
                shooting.StopShooting();
            }
        }
    }
}
