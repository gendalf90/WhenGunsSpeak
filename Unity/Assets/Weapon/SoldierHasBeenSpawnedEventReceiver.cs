using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierHasBeenSpawnedEventReceiver : MonoBehaviour
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
                .Receive<SoldierHasBeenSpawnedEvent>()
                .Where(message => message.SoldierId == identificator.SoldierId)
                .Do(message => spawning.Spawn())
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
