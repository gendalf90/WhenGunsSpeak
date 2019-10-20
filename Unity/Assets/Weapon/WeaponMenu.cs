using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Weapon
{
    public class WeaponMenu : MonoBehaviour
    {
        private Dropdown firstWeaponDropdown;
        private Listing listing;
        private Spawning spawning;

        private void Awake()
        {
            firstWeaponDropdown = transform
                .GetComponentsInChildren<Dropdown>()
                .First(component => component.name == "WeaponDropdown");
            listing = GetComponentInParent<Listing>();
            spawning = GetComponentInParent<Spawning>();
        }

        private void Start()
        {
            FillFirstWeaponList();
            ChooseSelectedFirstWeapon();
        }

        private void FillFirstWeaponList()
        {
            var firstWeaponNameList = new List<string>();

            listing.FillFirstWeaponList(firstWeaponNameList);

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

            spawning.SetWeaponName(selectedFirstWeapon);
        }
    }
}
