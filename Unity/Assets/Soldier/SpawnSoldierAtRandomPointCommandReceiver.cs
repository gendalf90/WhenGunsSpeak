using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SpawnSoldierCommandAtRandomPointReceiver : MonoBehaviour
    {
        private RandomSpawner spawner;
        private Identificator identificator;

        private void Awake()
        {
            spawner = GetComponent<RandomSpawner>();
            identificator = GetComponent<Identificator>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SpawnSoldierAtRandomPointCommand>()
                .Where(command => command.SoldierId == identificator.SoldierId)
                .Do(command => spawner.Spawn())
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
