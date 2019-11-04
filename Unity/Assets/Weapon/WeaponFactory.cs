using UnityEngine;

namespace Weapon
{
    public class WeaponFactory : MonoBehaviour
    {
        private GameObject prefab;

        private void Awake()
        {
            prefab = Resources.Load<GameObject>("Weapon");
        }

        public void CreateOfflinePlayerWeaponForSoldier(string weaponId, string soldierId)
        {
            var weapon = Instantiate(prefab);

            weapon.GetComponent<ChooseSoldierWeaponCommandReceiver>().SetSoldierId(soldierId);
            weapon.GetComponent<WeaponMenuCommandReceivers>().SetSoldierId(soldierId);
            weapon.GetComponent<SoldierPositionEventReceiver>().SetSoldierId(soldierId);
            weapon.GetComponent<SoldierHasBeenSpawnedEventReceiver>().SetSoldierId(soldierId);
            weapon.GetComponent<CreateOfflineShellCommandSender>().SetSoldierId(soldierId);
        }
    }
}
