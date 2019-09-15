using UnityEngine;

namespace Soldier
{
    public class SoldierFactory : MonoBehaviour
    {
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Soldier");
        }

        public void CreateOfflineUser(string soldierId)
        {
            InstantiateWithDefaults(soldierId);
        }

        private GameObject InstantiateWithDefaults(string soldierId)
        {
            var newSoldier = Instantiate(prefab);

            newSoldier.GetComponent<Identificator>().SoldierId = soldierId;
            newSoldier.GetComponent<SoldierPositionEventSender>().enabled = false;
            newSoldier.GetComponent<Mouse>().enabled = false;
            newSoldier.GetComponent<Keyboard>().enabled = false;

            return newSoldier;
        }
    }
}
