using Messages;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponMenuCommandReceivers : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private WeaponMenu menu;

        private void Awake()
        {
            menu = GetComponentInChildren<WeaponMenu>(true);
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<ShowWeaponMenuCommand>()
                .Where(message => message.SoldierId == soldierId)
                .Do(command => menu.Show())
                .TakeUntilDisable(this)
                .Subscribe();

            MessageBroker.Default
                .Receive<HideWeaponMenuCommand>()
                .Where(message => message.SoldierId == soldierId)
                .Do(command => menu.Hide())
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
