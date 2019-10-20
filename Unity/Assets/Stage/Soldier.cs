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

        [SerializeField]
        private bool isPlayer;

        private ISoldierSpawner spawner;
        private BeforeSpawnMenu beforeSpawnMenu;

        private void Awake()
        {
            spawner = GetComponentInParent<ISoldierSpawner>();
            beforeSpawnMenu = GetComponentInParent<BeforeSpawnMenu>();
        }

        private void Start()
        {
            PlanToSpawn();
            ShowTheBeforeSpawnMenuIfPlayer();
        }

        public void SetSoldierId(string id)
        {
            soldierId = id;
        }

        public bool HasSoldierId(string id)
        {
            return soldierId == id;
        }

        public void HasKilled()
        {
            kills++;
        }

        public void SetDied()
        {
            deads++;
            isSpawned = false;
            deadTime = Time.realtimeSinceStartup;

            PlanToSpawn();
            ShowTheBeforeSpawnMenuIfPlayer();
        }

        private void PlanToSpawn()
        {
            spawner.AddSpawnData(new ToSpawnSoldierData
            {
                SoldierId = soldierId,
                DeadTime = deadTime
            });
        }

        public void SetSpawned()
        {
            isSpawned = true;

            HideTheBeforeSpawnMenuIfPlayer();
        }

        public void SetAsPlayer()
        {
            isPlayer = true;
        }

        private void ShowTheBeforeSpawnMenuIfPlayer()
        {
            if(isPlayer)
            {
                beforeSpawnMenu.ShowForSoldier(soldierId);
            }
        }

        private void HideTheBeforeSpawnMenuIfPlayer()
        {
            if (isPlayer)
            {
                beforeSpawnMenu.HideForSoldier(soldierId);
            }
        }
    }
}
