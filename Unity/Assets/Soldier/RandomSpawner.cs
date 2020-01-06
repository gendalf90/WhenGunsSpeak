using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Soldier
{
    public class RandomSpawner : MonoBehaviour
    {
        [SerializeField]
        private float spawnDelaySeconds;

        private Spawning spawning;
        private SoldierSpawnPoint[] points;

        private void Awake()
        {
            spawning = GetComponent<Spawning>();
            points = FindObjectsOfType<SoldierSpawnPoint>();
        }

        public void Spawn()
        {
            StartCoroutine(StartSpawning());
        }

        private IEnumerator StartSpawning()
        {
            yield return new WaitForSeconds(spawnDelaySeconds);

            var spawnPoint = GetNextSpawnPoint();

            if (spawnPoint.HasValue)
            {
                spawning.Spawn(spawnPoint.Value);
            }
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
    }
}
