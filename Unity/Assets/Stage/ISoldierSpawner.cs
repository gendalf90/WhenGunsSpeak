using System.Collections.Generic;
using UnityEngine;

namespace Stage
{
    public class ToSpawnSoldierData
    {
        public string SoldierId { get; set; }

        public float DeadTime { get; set; }

        //public string Team { get; set; }

        //public bool IsLastInTeam { get; set; }
    }

    public class ReadyToSpawnSoldierData
    {
        public string SoldierId { get; set; }

        public Vector2 Position { get; set; }
    }

    public interface ISoldierSpawner
    {
        void AddSpawnData(ToSpawnSoldierData data);

        IEnumerable<ReadyToSpawnSoldierData> ConsumeReadyToSpawn();
    }
}
