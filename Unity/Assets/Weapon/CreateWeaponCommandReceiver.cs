using Messages;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class CreateWeaponCommandReceiver : MonoBehaviour
    {
        private WeaponFactory weaponFactory;

        private void Awake()
        {
            weaponFactory = GetComponent<WeaponFactory>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<CreateWeaponCommand>()
                .Do(command => weaponFactory.CreateWeaponForSoldier(command.WeaponId, command.SoldierId))
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
