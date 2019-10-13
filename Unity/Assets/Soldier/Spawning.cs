using UnityEngine;

namespace Soldier
{
    public class Spawning : MonoBehaviour
    {
        private Transform instance;

        private void Awake()
        {
            instance = transform.Find("Instance");
        }

        public void Spawn(Vector2 position)
        {
            transform.position = position;

            instance.gameObject.SetActive(true);
        }

        public void Unspawn()
        {

        }
    }
}
