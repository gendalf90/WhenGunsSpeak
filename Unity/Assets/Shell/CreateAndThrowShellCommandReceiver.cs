using Messages;
using UniRx;
using UnityEngine;

namespace Shell
{
    public class CreateAndThrowShellCommandReceiver : MonoBehaviour
    {
        private ShellFactory factory;

        private void Awake()
        {
            factory = GetComponent<ShellFactory>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<CreateAndThrowShellCommand>()
                .Select(command => new ShellData
                {
                    SoldierId = command.SoldierId,
                    ShellId = command.ShellId,
                    ShellKey = command.ShellKey,
                    Position = command.Position,
                    Rotation = command.Rotation
                })
                .Do(factory.CreateAndThrowShell)
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
