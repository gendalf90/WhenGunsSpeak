using Messages;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class WeaponMenuCommandReceivers : MonoBehaviour
    {
        private Identificator identificator;
        private WeaponMenu menu;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            menu = GetComponentInChildren<WeaponMenu>(true);
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<ShowWeaponMenuCommand>()
                .Where(message => message.SoldierId == identificator.SoldierId)
                .Do(command => menu.Show())
                .TakeUntilDisable(this)
                .Subscribe();

            MessageBroker.Default
                .Receive<HideWeaponMenuCommand>()
                .Where(message => message.SoldierId == identificator.SoldierId)
                .Do(command => menu.Hide())
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
