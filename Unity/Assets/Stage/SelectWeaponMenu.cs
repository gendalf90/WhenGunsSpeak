using Messages;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class SelectWeaponMenu : MonoBehaviour
    {
        [SerializeField]
        private bool isShowing;

        [SerializeField]
        private string playerSoldierId;

        [SerializeField]
        private bool isActive;

        private void FixedUpdate()
        {
            isShowing = Input.GetKey(KeyCode.Tab);
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenSetAsPlayerEvent>()
                .Do(e => playerSoldierId = e.SoldierId)
                .Do(e => isActive = true)
                .TakeUntilDisable(this)
                .Subscribe();

            MessageBroker.Default
                .Receive<SoldierHasBeenSpawnedEvent>()
                .Where(e => e.SoldierId == playerSoldierId)
                .Do(e => isActive = false)
                .TakeUntilDisable(this)
                .Subscribe();

            //когда soldier умирает тоже ставим isActive = true

            this.ObserveEveryValueChanged(menu => menu.isShowing)
                .Where(value => isActive)
                .Where(value => value)
                .Select(value => new ShowWeaponMenuCommand
                {
                    SoldierId = playerSoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();

            this.ObserveEveryValueChanged(menu => menu.isShowing)
                .Where(value => isActive)
                .Where(value => !value)
                .Select(value => new HideWeaponMenuCommand
                {
                    SoldierId = playerSoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();

            this.ObserveEveryValueChanged(menu => menu.isActive)
                .Where(value => !value)
                .Where(value => isShowing)
                .Select(value => new HideWeaponMenuCommand
                {
                    SoldierId = playerSoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
