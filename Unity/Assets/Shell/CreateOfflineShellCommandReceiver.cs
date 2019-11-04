using Messages;
using UniRx;
using UnityEngine;

namespace Shell
{
    public class CreateOfflineShellCommandReceiver : MonoBehaviour
    {
        private ShellFactory factory;

        private void Awake()
        {
            factory = GetComponent<ShellFactory>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<CreateOfflineShellCommand>()
                .Select(command => new CreateShellData
                {
                    SoldierId = command.SoldierId,
                    ShellId = command.ShellId,
                    ShellName = command.ShellName,
                    Position = command.Position,
                    Rotation = command.Rotation
                })
                .Do(factory.CreateOfflineShell)
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
