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
                .Receive<CreateSoldierCommand>()
                .Do(CreateOfflineSoldier)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void CreateOfflineSoldier(CreateSoldierCommand command)
        {
            factory.Create(command.SoldierId);
        }
    }
}
