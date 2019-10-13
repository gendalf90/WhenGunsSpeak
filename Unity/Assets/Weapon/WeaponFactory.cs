using UnityEngine;

namespace Weapon
{
    public class WeaponFactory : MonoBehaviour
    {
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Weapon");
        }

        public void CreateWeaponForSoldier(string soldierId)
        {
            Instantiate(prefab);
        }
    }
}
