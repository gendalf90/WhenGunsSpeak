using UnityEngine;

namespace Weapon
{
    public class Identificator : MonoBehaviour
    {
        [SerializeField]
        private string weaponId;

        [SerializeField]
        private string soldierId;

        public string WeaponId
        {
            get => weaponId;
            set => weaponId = value;
        }

        public string SoldierId
        {
            get => soldierId;
            set => soldierId = value;
        }
    }
}
