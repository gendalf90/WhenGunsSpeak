using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierPositionEventReceiver : MonoBehaviour
    {
        private Identificator identificator;
        private Position position;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            position = GetComponent<Position>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierPositionEvent>()
                .Where(message => message.SoldierId == identificator.SoldierId)
                .Do(SetPosition)
                .Do(SetFlip)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void SetPosition(SoldierPositionEvent message)
        {
            position.SetPosition(message.Position);
            position.AimTo(message.LookingPoint);
        }

        private void SetFlip(SoldierPositionEvent message)
        {
            if (message.IsRightLooking)
            {
                position.SetRightLooking();
            }

            if (message.IsLeftLooking)
            {
                position.SetLeftLooking();
            }
        }
    }
}
