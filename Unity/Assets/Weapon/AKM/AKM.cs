using System.Collections.Generic;
using UnityEngine;

namespace Weapon
{
    public class AKM : MonoBehaviour, IFlippable, IShootable, IListable, IChooseable
    {
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("AKM");
        }

        public void AddIfFirstWeapon(IList<string> list)
        {
            list.Add("AKM");
        }

        public void FlipToLeft()
        {
            throw new System.NotImplementedException();
        }

        public void FlipToRight()
        {
            throw new System.NotImplementedException();
        }

        public void StartShooting()
        {
            throw new System.NotImplementedException();
        }

        public void StopShooting()
        {
            throw new System.NotImplementedException();
        }

        public void ChooseIfNameIs(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
