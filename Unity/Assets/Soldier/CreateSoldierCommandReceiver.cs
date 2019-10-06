using Messages;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class CreateSoldierCommandReceiver : MonoBehaviour
    {
        private SoldierFactory factory;

        private void Awake()
        {
            factory = GetComponent<SoldierFactory>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<CreateOfflineSoldierCommand>()
                .Do(CreateOfflineSoldier)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void CreateOfflineSoldier(CreateOfflineSoldierCommand command)
        {
            factory.CreateOfflineSoldier(command.SoldierId);
        }
    }
}
