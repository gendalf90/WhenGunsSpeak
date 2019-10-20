using Messages;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class SoldierHasBeenCreatedEventReceiver : MonoBehaviour
    {
        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenCreatedEvent>()
                .Do(AddSoldier)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void AddSoldier(SoldierHasBeenCreatedEvent e)
        {
            var newSoldier = gameObject.AddComponent<Soldier>();

            newSoldier.SetSoldierId(e.SoldierId);

            if(e.IsPlayer)
            {
                newSoldier.SetAsPlayer();
            }
        }
    }
}
