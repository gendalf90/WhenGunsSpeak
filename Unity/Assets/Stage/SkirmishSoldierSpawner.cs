using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Stage
{
    public class SkirmishSoldierSpawner : MonoBehaviour, ISoldierSpawner
    {
        [SerializeField]
        private float spawnDelaySeconds;

        private List<ToSpawnSoldierData> plannedToSpawn = new List<ToSpawnSoldierData>();

        private SoldierSpawnPoint[] points;

        private void Awake()
        {
            points = GetComponentsInChildren<SoldierSpawnPoint>();
        }

        public IEnumerable<ReadyToSpawnSoldierData> ConsumeReadyToSpawn()
        {
            while(ConsumeNextReadyToSpawn(out var result))
            {
                yield return result;
            }
        }

        private bool ConsumeNextReadyToSpawn(out ReadyToSpawnSoldierData result)
        {
            result = null;

            var nextSpawnPoint = GetNextSpawnPoint();

            if(!nextSpawnPoint.HasValue)
            {
                return false;
            }

            var readyToSpawn = ConsumeNextPlannedToSpawn();

            if(readyToSpawn == null)
            {
                return false;
            }

            result = new ReadyToSpawnSoldierData
            {
                SoldierId = readyToSpawn.SoldierId,
                Position = nextSpawnPoint.Value
            };

            return true;
        }

        private ToSpawnSoldierData ConsumeNextPlannedToSpawn()
        {
            var result = plannedToSpawn.FirstOrDefault(CanSpawn);

            if(result != null)
            {
                plannedToSpawn.Remove(result);
            }

            return result;
        }

        private bool CanSpawn(ToSpawnSoldierData data)
        {
            return Time.realtimeSinceStartup - data.DeadTime > spawnDelaySeconds;
        }

        private Vector2? GetNextSpawnPoint()
        {
            if(points.Length == 0)
            {
                return null;
            }

            var nextSpawnPointIndex = Random.Range(0, points.Length);

            return points[nextSpawnPointIndex].transform.position;
        }

        public void AddSpawnData(ToSpawnSoldierData data)
        {
            plannedToSpawn.Add(data);
        }
    }
}
