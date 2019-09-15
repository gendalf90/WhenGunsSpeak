using Messages;
using UniRx;
using UnityEngine;
using UniRx.Triggers;

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
            this.UpdateAsObservable()
                .First()
                .Subscribe(count => PublishCreatedEvent());
        }

        private void PublishCreatedEvent()
        {
            MessageBroker.Default.Publish(new SoldierHasBeenCreatedEvent
            {
                SoldierId = identificator.SoldierId
            });
        }
    }
}
