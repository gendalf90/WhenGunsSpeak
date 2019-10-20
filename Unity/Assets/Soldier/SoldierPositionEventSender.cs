using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierPositionEventSender : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private Looking looking;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        private void Awake()
        {
            looking = GetComponentInParent<Looking>();
        }

        public void FixedUpdate()
        {
            var message = new SoldierPositionEvent
            {
                SoldierId = soldierId,
                Position = transform.position,
            };

            looking.FillMessage(message);

            MessageBroker.Default.Publish(message);
        }
    }
}
