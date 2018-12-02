namespace Server
{
    public class SendMessageCommand
    {
        public SendMessageCommand(byte[] data)
        {
            Data = data;
        }

        public byte[] Data { get; private set; }
    }
}
