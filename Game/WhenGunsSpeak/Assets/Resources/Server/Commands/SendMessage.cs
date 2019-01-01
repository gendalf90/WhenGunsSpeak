using BinaryProcessing;

namespace Server
{
    public class SendMessageCommand
    {
        public SendMessageCommand(Binary data)
        {
            Data = data;
        }

        public Binary Data { get; private set; }
    }
}
