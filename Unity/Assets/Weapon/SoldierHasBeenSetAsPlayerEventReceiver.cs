using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierHasBeenSetAsPlayerEventReceiver : MonoBehaviour
    {
        private Player player;
        private Identificator identificator;

        private void Awake()
        {
            player = GetComponent<Player>();
            identificator = GetComponent<Identificator>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenSetAsPlayerEvent>()
                .Where(command => command.SoldierId == identificator.SoldierId)
                .Do(command => player.SetAsPlayer())
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
