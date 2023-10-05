using ClientTwo.DTO.Interface;

namespace ClientTwo.DTO
{
    public class GameMessage : IMessageType
    {
        public string Position { get; set; }

        public string Text { get; set; }

        public bool ClientPlayed { get; set; }

        public string MessageType => "GameMessage";
    }
}
