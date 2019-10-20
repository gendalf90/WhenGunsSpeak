using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class Listing : MonoBehaviour
    {
        private IListable[] listableWeapons;

        private void Awake()
        {
            listableWeapons = GetComponentsInChildren<IListable>(true);
        }

        public void FillFirstWeaponList(List<string> list)
        {
            foreach (var weapon in listableWeapons)
            {
                weapon.AddIfFirstWeapon(list);
            }
        }
    }
}
