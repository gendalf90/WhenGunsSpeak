namespace Server
{
    public class SendMessageCommand
    {
        public SendMessageCommand(string json)
        {
            Json = json;
        }

        public string Json { get; private set; }
    }
}
