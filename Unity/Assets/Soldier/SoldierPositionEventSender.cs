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
            looking = GetComponentInParent<Looking>();
            identificator = GetComponentInParent<Identificator>();
        }

        public void Update()
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
