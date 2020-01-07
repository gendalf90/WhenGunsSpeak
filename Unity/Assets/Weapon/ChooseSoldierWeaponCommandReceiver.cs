using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class ChooseSoldierWeaponCommandReceiver : MonoBehaviour
    {
        private Identificator identificator;
        private Spawning spawning;

        private void Awake()
        {
            identificator = GetComponent<Identificator>();
            spawning = GetComponent<Spawning>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<ChooseSoldierWeaponCommand>()
                .Where(command => command.SoldierId == identificator.SoldierId)
                .Do(command => spawning.SetWeaponName(command.WeaponName))
                .TakeUntilDisable(this)
                .Subscribe();
        }
    }
}
