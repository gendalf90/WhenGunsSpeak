using Messages;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class CreateSoldierWeaponCommandReceiver : MonoBehaviour
    {
        private WeaponFactory weaponFactory;

        private void Awake()
        {
            weaponFactory = GetComponent<WeaponFactory>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenCreatedEvent>()
                .Do(command => weaponFactory.CreateWeaponForSoldier(command.SoldierId))
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
