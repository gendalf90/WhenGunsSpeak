using UnityEngine;

namespace Weapon
{
    public class Identificator : MonoBehaviour
    {
        [SerializeField]
        private string weaponId;

        public string WeaponId
        {
            get => weaponId;
            set => weaponId = value;
        }
    }
}
