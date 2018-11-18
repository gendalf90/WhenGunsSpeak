namespace Server
{
    public class StartNewRoomCommand
    {
        public StartNewRoomCommand(string header)
        {
            Header = header;
        }

        public string Header { get; private set; }
    }
}
