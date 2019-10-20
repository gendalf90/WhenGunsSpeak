using Messages;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class WeaponMenuCommandSenders : MonoBehaviour
    {
        private void OnEnable()
        {
            GetComponent<BeforeSpawnMenu>()
                .Where(data => data.IsShow)
                .Select(data => new ShowWeaponMenuCommand
                {
                    SoldierId = data.SoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();

            GetComponent<BeforeSpawnMenu>()
                .Where(data => data.IsHide)
                .Select(data => new HideWeaponMenuCommand
                {
                    SoldierId = data.SoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
