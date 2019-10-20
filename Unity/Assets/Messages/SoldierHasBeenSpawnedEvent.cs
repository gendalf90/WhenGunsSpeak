namespace Messages
{
    public class SoldierHasBeenSpawnedEvent
    {
        public string SoldierId { get; set; }

        public bool IsPlayer { get; set; }
    }
}
