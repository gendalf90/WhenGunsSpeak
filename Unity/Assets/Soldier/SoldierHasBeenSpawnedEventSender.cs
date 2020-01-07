using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenSpawnedEventSender : MonoBehaviour
    {
        private Identificator identificator;

        private void Awake()
        {
            identificator = GetComponentInParent<Identificator>();
        }

        private void OnEnable()
        {
            MessageBroker.Default.Publish(new SoldierHasBeenSpawnedEvent
            {
                SoldierId = identificator.SoldierId
            });
        }
    }
}
