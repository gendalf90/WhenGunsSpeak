using BinaryProcessing;

namespace Server
{
    class SendMessageAtRoomCommand
    {
        public SendMessageAtRoomCommand(Binary data)
        {
            Data = data;
        }

        public Binary Data { get; private set; }
    }
}
