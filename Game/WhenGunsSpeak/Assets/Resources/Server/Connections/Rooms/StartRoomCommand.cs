namespace Server
{
    class StartRoomCommand
    {
        public StartRoomCommand(string header)
        {
            Header = header;
        }

        public string Header { get; private set; }
    }
}
