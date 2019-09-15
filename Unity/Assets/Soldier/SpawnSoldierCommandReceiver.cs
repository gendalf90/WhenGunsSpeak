using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SpawnSoldierCommandReceiver : MonoBehaviour
    {
        private Identificator identificator;
        private Spawning spawning;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            spawning = GetComponent<Spawning>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SpawnSoldierCommand>()
                .Where(message => message.SoldierId == identificator.SoldierId)
                .Do(Spawn)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void Spawn(SpawnSoldierCommand command)
        {
            spawning.Spawn(command.Position);
        }
    }
}
