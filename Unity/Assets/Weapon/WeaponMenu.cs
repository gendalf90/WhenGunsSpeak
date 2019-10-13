using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Weapon
{
    public class WeaponMenu : MonoBehaviour
    {
        private Dropdown firstWeaponDropdown;
        private IListable[] listableWeapons;
        private IChooseable[] createableWeapons;

        private void Awake()
        {
            firstWeaponDropdown = transform
                .GetComponentsInChildren<Dropdown>()
                .First(component => component.name == "WeaponDropdown");

            listableWeapons = GetComponentsInParent<IListable>();
            createableWeapons = GetComponentsInParent<IChooseable>();
        }

        private void Start()
        {
            FillFirstWeaponList();
        }

        private void FillFirstWeaponList()
        {
            var firstWeaponNameList = new List<string>();

            foreach(var weapon in listableWeapons)
            {
                weapon.AddIfFirstWeapon(firstWeaponNameList);
            }

            firstWeaponDropdown.AddOptions(firstWeaponNameList);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void ChooseSelectedFirstWeapon()
        {
            var selectedFirstWeapon = firstWeaponDropdown.captionText.text;

            foreach(var weapon in createableWeapons)
            {
                weapon.ChooseIfNameIs(selectedFirstWeapon);
            }
        }
    }
}
