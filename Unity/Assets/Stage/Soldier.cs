using UnityEngine;

namespace Stage
{
    public class Soldier : MonoBehaviour
    {
        [SerializeField]
        private string soldierId;

        [SerializeField]
        private int kills;

        [SerializeField]
        private int deads;

        [SerializeField]
        private bool isSpawned;

        [SerializeField]
        private float deadTime;

        private ISoldierSpawner[] spawners;

        private void Awake()
        {
            spawners = GetComponentsInParent<ISoldierSpawner>();
        }

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        public void HasKilled()
        {
            kills++;
        }

        public void HasDied()
        {
            deads++;
            isSpawned = false;
            deadTime = Time.realtimeSinceStartup;

            PlanToSpawn();
        }

        private void PlanToSpawn()
        {
            foreach (var spawner in spawners)
            {
                spawner.AddSpawnData(new ToSpawnSoldierData
                {
                    SoldierId = soldierId,
                    DeadTime = deadTime
                });
            }
        }

        public void HasSpawned()
        {
            isSpawned = true;
        }
    }
}
