using ClientOne.DTO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOne.DTO
{
    public class User : IMessageType
    {
        public string Nickname { get; set; }
        public string ChosenSymbol { get; set; }

        public string MessageType => "UserMessage";
    }
}
