using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenCreatedEventSender : MonoBehaviour
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

        private void Start()
        {
            MessageBroker.Default.Publish(new SoldierHasBeenCreatedEvent
            {
                SoldierId = soldierId,
                IsPlayer = isPlayer
            });
        }
    }
}
