using Messages;
using UniRx;
using UnityEngine;
using System.Linq;

namespace Stage
{
    public class SpawnSoldierCommandSender : MonoBehaviour
    {
        private ISoldierSpawner[] spawners;

        private void Awake()
        {
            spawners = GetComponents<ISoldierSpawner>();
        }

        public void Update()
        {
            var spawnSoldierCommands = spawners
                .SelectMany(spawner => spawner.ConsumeReadyToSpawn())
                .Select(spawnData => new SpawnSoldierCommand
                {
                    SoldierId = spawnData.SoldierId,
                    Position = spawnData.Position
                });

            foreach(var command in spawnSoldierCommands)
            {
                MessageBroker.Default.Publish(command);
            }
        }
    }
}
