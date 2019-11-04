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

        public void CreateOfflinePlayer(string soldierId)
        {
            var newSoldier = InstantiateWithDefaults(soldierId);

            newSoldier.GetComponent<SoldierHasBeenCreatedEventSender>().SetAsPlayer();
        }

        private GameObject InstantiateWithDefaults(string soldierId)
        {
            var newSoldier = Instantiate(prefab);

            newSoldier.GetComponent<Movement>().SetDefaultForce();
            newSoldier.GetComponent<SpawnSoldierCommandReceiver>().SetSoldierId(soldierId);
            newSoldier.GetComponent<SoldierHasBeenCreatedEventSender>().SetSoldierId(soldierId);

            newSoldier.GetComponentInChildren<SoldierHasBeenSpawnedEventSender>(true).SetSoldierId(soldierId);
            newSoldier.GetComponentInChildren<SoldierPositionEventSender>(true).SetSoldierId(soldierId);

            return newSoldier;
        }
    }
}
