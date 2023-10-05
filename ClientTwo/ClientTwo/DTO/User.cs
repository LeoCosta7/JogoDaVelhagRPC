using ClientTwo.DTO.Interface;

namespace ClientTwo.DTO
{
    public class User : IMessageType
    {
        public string Nickname { get; set; }
        public string ChosenSymbol { get; set; }

        public string MessageType => "UserMessage";
    }
}
