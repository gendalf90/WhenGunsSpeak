using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class SoldierHasBeenSpawnedEventReceiver : MonoBehaviour
    {
        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenSpawnedEvent>()
                .Do(SpawnSoldierFromEvent)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void SpawnSoldierFromEvent(SoldierHasBeenSpawnedEvent e)
        {
            var soldierToSpawn = GetComponentsInChildren<Soldier>().FirstOrDefault(soldier => soldier.HasSoldierId(e.SoldierId));

            if(soldierToSpawn != null)
            {
                soldierToSpawn.SetSpawned();
            }
        }
    }
}
