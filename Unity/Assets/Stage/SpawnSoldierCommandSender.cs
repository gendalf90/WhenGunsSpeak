using Messages;
using UniRx;
using UnityEngine;
using System.Linq;

namespace Stage
{
    public class SpawnSoldierCommandSender : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<ISoldierSpawner>()
                .Select(spawnData => new SpawnSoldierCommand
                {
                    SoldierId = spawnData.SoldierId,
                    Position = spawnData.Position
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
