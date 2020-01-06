using System;
using UniRx;
using UnityEngine;

namespace Soldier
{
    public class PlayerState
    {
        public bool IsPlayer { get; set; }
    }

    public class Player : MonoBehaviour, IObservable<PlayerState>
    {
        [SerializeField]
        private bool isPlayer;

        private Mouse mouse;
        private Keyboard keyboard;

        private void Awake()
        {
            mouse = GetComponentInChildren<Mouse>(true);
            keyboard = GetComponentInChildren<Keyboard>(true);
        }

        public void SetAsPlayer()
        {
            mouse.enabled = true;
            keyboard.enabled = true;
            isPlayer = true;
        }

        public IDisposable Subscribe(IObserver<PlayerState> observer)
        {
            return this.ObserveEveryValueChanged(player => player.isPlayer)
                .Select(value => new PlayerState { IsPlayer = value })
                .Subscribe(observer);
        }
    }
}
