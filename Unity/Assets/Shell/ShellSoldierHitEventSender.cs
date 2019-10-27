using Messages;
using UniRx;
using UnityEngine;

namespace Shell
{
    public class ShellSoldierHitEventSender : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            MessageBroker.Default.Publish(new ShellSoldierHitEvent
            {
                SoldierId = soldierId
            });
        }
    }
}
