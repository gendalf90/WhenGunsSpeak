using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenSpawnedEventSender : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        [SerializeField]
        private bool isPlayer;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        public void SetAsPlayer()
        {
            isPlayer = true;
        }

        private void OnEnable()
        {
            MessageBroker.Default.Publish(new SoldierHasBeenSpawnedEvent
            {
                SoldierId = soldierId,
                IsPlayer = isPlayer
            });
        }
    }
}
