using UnityEngine;

namespace Soldier
{
    public class Spawning : MonoBehaviour
    {
        public void Spawn(Vector2 position)
        {
            transform.position = position;

            gameObject.SetActive(true);
        }
    }
}
