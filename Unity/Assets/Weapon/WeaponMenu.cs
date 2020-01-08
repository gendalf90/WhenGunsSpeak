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
        private GameObject canvas;

        private void Awake()
        {
            firstWeaponDropdown = GetComponentsInChildren<Dropdown>(true).First(component => component.name == "WeaponDropdown");
            canvas = transform.Find("Canvas").gameObject;
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
            canvas.SetActive(true);
        }

        public void Hide()
        {
            canvas.SetActive(false);
        }

        public void ChooseSelectedFirstWeapon()
        {
            var selectedFirstWeapon = firstWeaponDropdown.captionText.text;

            spawning.SetWeaponName(selectedFirstWeapon);
        }
    }
}
