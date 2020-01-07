using Messages;
using UniRx;
using UnityEngine;

namespace Shell
{
    // удалять shell после попадания или нет решает тот, в кого попали
    public class ShellSoldierHitEventSender : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var identificator = other.GetComponentInParent<Identificator>();

            MessageBroker.Default.Publish(new ShellSoldierHitEvent
            {
                ShellId = identificator.ShellId,
                ToSoldierId = soldierId,
                FromSoldierId = identificator.SoldierId
            });
        }
    }
}
