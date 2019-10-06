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
                .Where(command => command.SoldierId == identificator.SoldierId)
                .Do(command => spawning.Spawn(command.Position))
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
