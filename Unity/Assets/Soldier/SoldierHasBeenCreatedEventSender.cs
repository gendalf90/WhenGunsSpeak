using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenCreatedEventSender : MonoBehaviour
    {
        private Identificator identificator;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
        }

        private void Start()
        {
            MessageBroker.Default.Publish(new SoldierHasBeenCreatedEvent
            {
                SoldierId = identificator.SoldierId
            });
        }
    }
}
