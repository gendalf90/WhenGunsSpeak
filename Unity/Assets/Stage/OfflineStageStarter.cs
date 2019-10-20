using Messages;
using System;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class OfflineStageStarter : MonoBehaviour
    {
        private void Start()
        {
            var soldierId = Guid.NewGuid().ToString();
            var weaponId = Guid.NewGuid().ToString();

            MessageBroker.Default.Publish(new CreateOfflinePlayerCommand
            {
                SoldierId = soldierId
            });

            MessageBroker.Default.Publish(new CreateWeaponCommand
            {
                SoldierId = soldierId,
                WeaponId = weaponId
            });
        }
    }
}
