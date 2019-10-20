namespace Messages
{
    public class SoldierHasBeenCreatedEvent
    {
        public string SoldierId { get; set; }

        public bool IsPlayer { get; set; }
    }
}
