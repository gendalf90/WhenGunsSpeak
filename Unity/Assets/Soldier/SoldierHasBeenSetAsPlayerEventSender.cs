using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenSetAsPlayerEventSender : MonoBehaviour
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
            player
                .Where(state => state.IsPlayer)
                .Select(state => new SoldierHasBeenSetAsPlayerEvent
                {
                    SoldierId = identificator.SoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
