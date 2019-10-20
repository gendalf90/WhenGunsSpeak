using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierHasBeenSpawnedEventReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private Spawning spawning;

        private void Awake()
        {
            spawning = GetComponent<Spawning>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenSpawnedEvent>()
                .Where(message => message.SoldierId == soldierId)
                .Do(message => spawning.Spawn())
                .TakeUntilDisable(this)
                .Subscribe();
        }

        public void SetSoldierId(string soldierId)
        {
            this.soldierId = soldierId;
        }
    }
}
