using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class CreateOfflineShellCommandSender : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private IShootable[] shootables;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        private void Awake()
        {
            shootables = GetComponentsInChildren<IShootable>(true);
        }

        private void OnEnable()
        {
            foreach(var shootable in shootables)
            {
                shootable
                    .Select(data => new CreateOfflineShellCommand
                    {
                        SoldierId = soldierId,
                        ShellId = data.ShellId,
                        ShellName = data.ShellName,
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
