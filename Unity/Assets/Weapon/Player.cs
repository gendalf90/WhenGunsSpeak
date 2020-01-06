using UnityEngine;

namespace Weapon
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private bool isPlayer;

        private Mouse mouse;

        private void Awake()
        {
            mouse = GetComponent<Mouse>();
        }

        public void SetAsPlayer()
        {
            mouse.enabled = true;
            isPlayer = true;
        }
    }
}
