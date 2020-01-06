using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class SoldierRandomPointSpawner : MonoBehaviour
    {
        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenSetAsPlayerEvent>()
                .Select(e => new SpawnSoldierAtRandomPointCommand
                {
                    SoldierId = e.SoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();

            //когда soldier умирает тоже отправляем запрос на spawn
        }
    }
}
