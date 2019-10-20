using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SpawnSoldierCommandReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private Spawning spawning;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        private void Awake()
        {
            spawning = GetComponent<Spawning>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SpawnSoldierCommand>()
                .Where(command => command.SoldierId == soldierId)
                .Do(command => spawning.Spawn(command.Position))
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
