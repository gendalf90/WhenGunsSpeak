using UnityEngine;

namespace Soldier
{
    public class SoldierFactory : MonoBehaviour
    {
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Soldier");

            prefab.SetActive(false);
        }

        public void CreateOfflineSoldier(string soldierId)
        {
            var newSoldier = InstantiateWithDefaults(soldierId);
        }

        private GameObject InstantiateWithDefaults(string soldierId)
        {
            var newSoldier = Instantiate(prefab);

            newSoldier.GetComponent<Identificator>().SoldierId = soldierId;

            return newSoldier;
        }
    }
}
