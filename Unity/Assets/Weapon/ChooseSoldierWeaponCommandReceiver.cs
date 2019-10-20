﻿using Messages;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Weapon
{
    public class ChooseSoldierWeaponCommandReceiver : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        private Spawning spawning;

        private void Awake()
        {
            spawning = GetComponent<Spawning>();
        }

        private void OnEnable()
        {
            MessageBroker.Default
                .Receive<ChooseSoldierWeaponCommand>()
                .Where(command => command.SoldierId == soldierId)
                .Do(command => spawning.SetWeaponName(command.WeaponName))
                .TakeUntilDisable(this)
                .Subscribe();
        }

        public void SetSoldierId(string soldierId)
        {
            this.soldierId = soldierId;
        }
    }
}
