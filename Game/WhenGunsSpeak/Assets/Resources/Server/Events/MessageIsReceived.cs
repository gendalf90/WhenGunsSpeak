namespace Server
{
    public class MessageIsReceivedEvent
    {
        public MessageIsReceivedEvent(string json)
        {
            Json = json;
        }

        public string Json { get; private set; }
    }
}
