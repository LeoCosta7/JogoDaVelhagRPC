using ClientOne.DTO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientOne.DTO
{
    public class GameMessage : IMessageType
    {
        public string Position { get; set; }

        public string Text { get; set; }

        public bool ClientPlayed { get; set; }

        public string MessageType => "GameMessage";
    }
}
