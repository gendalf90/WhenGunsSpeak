using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SetSoldierAsPlayerCommandReceiver : MonoBehaviour
    {
        private Identificator identificator;
        private Player player;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SetSoldierAsPlayerCommand>()
                .Where(command => command.SoldierId == identificator.SoldierId)
                .Do(command => player.SetAsPlayer())
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
