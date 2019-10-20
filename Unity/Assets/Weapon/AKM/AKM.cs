using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Weapon
{
    public class AKM : MonoBehaviour, IFlippable, IShootable, IListable, ISpawnable
    {
        private const string Name = "AKM";

        private GameObject prefab;

        private Transform bodyTransform;

        private void Awake()
        {
            bodyTransform = transform.Find("Body");
        }

        public void AddIfFirstWeapon(IList<string> list)
        {
            list.Add(Name);
        }

        public void FlipToLeft()
        {
            bodyTransform.SetFlipY(true);
        }

        public void FlipToRight()
        {
            bodyTransform.SetFlipY(false);
        }

        public void StartShooting()
        {
            
        }

        public void StopShooting()
        {
            
        }

        public void SpawnIfNameIs(string name)
        {
            if(name == Name)
            {
                gameObject.SetActive(true);
            }
        }

        public void UnspawnIfNameIs(string name)
        {
            if (name == Name)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
