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
            MessageBroker.Default.Publish(new CreateOfflineSoldierCommand
            {
                SoldierId = Guid.NewGuid().ToString()
            });
        }
    }
}
