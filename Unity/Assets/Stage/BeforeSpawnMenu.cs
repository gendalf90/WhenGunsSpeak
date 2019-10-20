using System;
using UniRx;
using UnityEngine;

namespace Stage
{
    public class BeforeSpawnMenuActionData
    {
        public string SoldierId { get; set; }

        public bool IsShow { get; set; }

        public bool IsHide { get; set; }
    }

    public class BeforeSpawnMenu : MonoBehaviour, IObservable<BeforeSpawnMenuActionData>
    {
        private Subject<BeforeSpawnMenuActionData> subject = new Subject<BeforeSpawnMenuActionData>();

        public void ShowForSoldier(string soldierId)
        {
            subject.OnNext(new BeforeSpawnMenuActionData
            {
                SoldierId = soldierId,
                IsShow = true
            });
        }

        public void HideForSoldier(string soldierId)
        {
            subject.OnNext(new BeforeSpawnMenuActionData
            {
                SoldierId = soldierId,
                IsHide = true
            });
        }

        public IDisposable Subscribe(IObserver<BeforeSpawnMenuActionData> observer)
        {
            return subject.Subscribe(observer);
        }
    }
}
