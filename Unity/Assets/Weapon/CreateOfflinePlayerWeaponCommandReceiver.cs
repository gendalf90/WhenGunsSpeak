using Messages;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class CreateOfflinePlayerWeaponCommandReceiver : MonoBehaviour
    {
        private WeaponFactory weaponFactory;

        private void Awake()
        {
            weaponFactory = GetComponent<WeaponFactory>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<CreateOfflinePlayerWeaponCommand>()
                .Do(command => weaponFactory.CreateOfflinePlayerWeaponForSoldier(command.WeaponId, command.SoldierId))
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
