using Messages;
using UniRx;
using UnityEngine;
using Utils;

namespace Stage
{
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField]
        private string newSoldierId;

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<SoldierHasBeenCreatedEvent>()
                .Select(e => new SetSoldierAsPlayerCommand
                {
                    SoldierId = e.SoldierId
                })
                .Do(MessageBroker.Default.Publish)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void Start()
        {
            MessageBroker.Default.Publish(new CreateSoldierCommand
            {
                SoldierId = GetOrCreateNewId()
            });
        }

        private string GetOrCreateNewId()
        {
            return string.IsNullOrWhiteSpace(newSoldierId) ? IdGenerator.Generate() : newSoldierId;
        }
    }
}
