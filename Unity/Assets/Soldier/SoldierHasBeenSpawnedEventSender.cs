using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenSpawnedEventSender : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        private void OnEnable()
        {
            MessageBroker.Default.Publish(new SoldierHasBeenSpawnedEvent
            {
                SoldierId = soldierId
            });
        }
    }
}
