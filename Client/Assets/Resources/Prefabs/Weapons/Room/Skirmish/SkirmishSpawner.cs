//using UnityEngine;
//using System.Collections;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//public class SkirmishSpawnEventArgs : EventArgs
//{
//    public SkirmishSpawnEventArgs(Guid id, Vector2 location)
//    {
//        Id = id;
//        Location = location;
//    }

//    public Guid Id { get; private set; }
//    public Vector2 Location { get; private set; }
//}

//public class SkirmishSpawner : MonoBehaviour
//{
//    [SerializeField]
//    private float beforeSpawnWaitTimeInSeconds;

//    private Dictionary<Guid, float> startSpawnTimeSinceStartupInSeconds;
//    private Queue<GameObject> respawns;

//    public SkirmishSpawner()
//    {
//        startSpawnTimeSinceStartupInSeconds = new Dictionary<Guid, float>();
//    }

//    private void Awake()
//    {
//        respawns = new Queue<GameObject>(GameObject.FindGameObjectsWithTag("Respawn"));
//    }

//    private void Update()
//    {
//        var toSpawnIds = startSpawnTimeSinceStartupInSeconds.Where(x => IsSpawnTime(x.Value))
//                                                            .Select(x => x.Key)
//                                                            .ToArray();
//        foreach(var spawnId in toSpawnIds)
//        {
//            startSpawnTimeSinceStartupInSeconds.Remove(spawnId);
//            var spawnLocation = GetNextSpawnLocation();
//            OnSpawnEvent.SafeRaise(this, new SkirmishSpawnEventArgs(spawnId, spawnLocation));
//        }
//    }

//    private bool IsSpawnTime(float waitStartTime)
//    {
//        return Time.realtimeSinceStartup - waitStartTime > beforeSpawnWaitTimeInSeconds;
//    }

//    private Vector2 GetNextSpawnLocation()
//    {
//        var respawn = respawns.Dequeue();
//        respawns.Enqueue(respawn);
//        return respawn.transform.position;
//    }

//    public void Respawn(Guid id)
//    {
//        startSpawnTimeSinceStartupInSeconds.Add(id, Time.realtimeSinceStartup);
//    }

//    public void StopRespawn(Guid id)
//    {
//        startSpawnTimeSinceStartupInSeconds.Remove(id);
//    }

//    public IEnumerable<Guid> ToRespawnIds
//    {
//        get
//        {
//            return startSpawnTimeSinceStartupInSeconds.Select(x => x.Key);
//        }
//    }

//    public event EventHandler<SkirmishSpawnEventArgs> OnSpawnEvent;
//}
