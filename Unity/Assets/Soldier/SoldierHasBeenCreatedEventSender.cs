using Messages;
using System;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class SoldierHasBeenCreatedEventSender : MonoBehaviour
    {
        private GameObject[] instantiatedSoldiers = Array.Empty<GameObject>();

        private void Update()
        {
            var currentSoldiers = GameObject.FindGameObjectsWithTag("Soldier");
            var newSoldierIds = currentSoldiers
                .Except(instantiatedSoldiers)
                .Select(soldier => soldier.GetComponent<Identificator>())
                .Select(id => id.SoldierId);

            foreach (var newSoldierId in newSoldierIds)
            {
                MessageBroker.Default.Publish(new SoldierHasBeenCreatedEvent
                {
                    SoldierId = newSoldierId
                });
            }

            instantiatedSoldiers = currentSoldiers;
        }
    }
}
