using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class SoldierPositionEventReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private Position position;

        private void Awake()
        {
            position = GetComponent<Position>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierPositionEvent>()
                .Where(message => message.SoldierId == soldierId)
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

        public void SetSoldierId(string soldierId)
        {
            this.soldierId = soldierId;
        }
    }
}
