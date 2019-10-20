using UnityEngine;

namespace Weapon
{
    public class Spawning : MonoBehaviour
    {
        [SerializeField]
        private string weaponNameToSpawn;

        private ISpawnable[] spawnables;

        private void Awake()
        {
            spawnables = GetComponentsInChildren<ISpawnable>(true);
        }

        public void SetWeaponName(string name)
        {
            weaponNameToSpawn = name;
        }

        public void Spawn()
        {
            foreach(var weapon in spawnables)
            {
                weapon.SpawnIfNameIs(weaponNameToSpawn);
            }
        }
    }
}
