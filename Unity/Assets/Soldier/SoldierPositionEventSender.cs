using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierPositionEventSender : MonoBehaviour
    {
        private Identificator identificator;
        private Looking looking;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            looking = GetComponent<Looking>();
        }

        public void FixedUpdate()
        {
            var message = new SoldierPositionEvent
            {
                SoldierId = identificator.SoldierId,
                Position = transform.position,
            };

            looking.FillMessage(message);

            MessageBroker.Default.Publish(message);
        }
    }
}
