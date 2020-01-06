using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class CreateShellCommandSender : MonoBehaviour
    {
        private Identificator identificator;
        private IShootable[] shootables;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            shootables = GetComponentsInChildren<IShootable>(true);
        }

        private void OnEnable()
        {
            foreach(var shootable in shootables)
            {
                shootable
                    .Select(data => new CreateAndThrowShellCommand
                    {
                        SoldierId = identificator.SoldierId,
                        ShellId = data.ShellId,
                        ShellKey = data.ShellKey,
                        Position = data.Position,
                        Rotation = data.Rotation
                    })
                    .Do(MessageBroker.Default.Publish)
                    .TakeUntilDisable(this)
                    .Subscribe();
            }
        }
    }
}
