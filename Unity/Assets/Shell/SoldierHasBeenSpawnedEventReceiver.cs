using Messages;
using UniRx;
using UnityEngine;

namespace Shell
{
    public class SoldierHasBeenSpawnedEventReceiver : MonoBehaviour
    {
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("SoldierShellBorder");
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenSpawnedEvent>()
                .Do(CreateShellBorderForSoldier)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void CreateShellBorderForSoldier(SoldierHasBeenSpawnedEvent e)
        {
            var border = Instantiate(prefab);

            border.GetComponent<ShellSoldierHitEventSender>().SetSoldierId(e.SoldierId);
            border.GetComponent<SoldierPositionEventReceiver>().SetSoldierId(e.SoldierId);
        }
    }
}
