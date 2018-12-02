namespace Server
{
    public class MessageIsReceivedEvent
    {
        public MessageIsReceivedEvent(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; private set; }
    }
}
