using UnityEngine;

namespace Soldier
{
    public class Mouse : MonoBehaviour
    {
        private Looking looking;

        private void Awake()
        {
            looking = GetComponent<Looking>();
        }

        private void FixedUpdate()
        {
            var mousePosition = Input.mousePosition;
            var worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            looking.LookToPoint(worldPosition);
        }
    }
}
