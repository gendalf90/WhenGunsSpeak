using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class ChooseSoldierWeaponCommandReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private IChooseable[] chooseables;

        private void Awake()
        {
            chooseables = GetComponentsInChildren<IChooseable>(true);
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<ChooseSoldierWeaponCommand>()
                .Where(command => command.SoldierId == soldierId)
                .Do(ChooseWeapon)
                .TakeUntilDisable(this)
                .Subscribe();
        }

        private void ChooseWeapon(ChooseSoldierWeaponCommand command)
        {
            foreach(var weapon in chooseables)
            {
                weapon.ChooseIfNameIs(command.WeaponName);
            }
        }
    }
}
