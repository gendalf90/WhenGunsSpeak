using UnityEngine;

namespace Shell
{
    public class Position : MonoBehaviour
    {
        public void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public void SetRotation(float angleToRotate)
        {
            transform.rotation = Quaternion.Euler(0, 0, angleToRotate);
        }
    }
}
