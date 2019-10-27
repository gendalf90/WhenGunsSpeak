using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Shell
{
    public class SoldierPositionEventReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierPositionEvent>()
                .Where(message => message.SoldierId == soldierId)
                .Do(e => transform.position = e.Position)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        public void SetSoldierId(string soldierId)
        {
            this.soldierId = soldierId;
        }
    }
}
