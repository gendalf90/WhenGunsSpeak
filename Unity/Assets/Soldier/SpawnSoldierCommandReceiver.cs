using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SpawnSoldierCommandReceiver : MonoBehaviour
    {
        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SpawnSoldierCommand>()
                .Do(Spawn)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void Spawn(SpawnSoldierCommand command)
        {
            var soldierToSpawn = GameObject.FindGameObjectsWithTag("Soldier")
                .Select(soldier => new
                {
                    Identificator = soldier.GetComponent<Identificator>(),
                    Spawning = soldier.GetComponent<Spawning>()
                })
                .FirstOrDefault(data => data.Identificator.SoldierId == command.SoldierId);

            if (soldierToSpawn != null)
            {
                soldierToSpawn.Spawning.Spawn(command.Position);
            }
        }
    }
}
