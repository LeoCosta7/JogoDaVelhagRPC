using ClientTwo.DTO;
using ClientTwo.DTO.Interface;
using ClientTwo.Entity;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace ClientTwo
{
    public partial class Form1 : Form
    {
        private GrpcChannel channel;
        private Greeter.GreeterClient client;

        public AsyncDuplexStreamingCall<PlayerChatInfoRequest, PlayerChatInfoResponse> streamingCall;
        public IAsyncStreamWriter<PlayerChatInfoRequest> requestStream;
        public IAsyncStreamReader<PlayerChatInfoResponse> responseStream;
        public AsyncDuplexStreamingCall<PlayerGameDataRequest, PlayerGameDataResponse> streamingPlayerCall;
        public IAsyncStreamWriter<PlayerGameDataRequest> requestPlayerStream;
        public IAsyncStreamReader<PlayerGameDataResponse> responsePlayerStream;

        public AsyncDuplexStreamingCall<PlayerInfoRequest, PlayerInfoResponse> streamingPlayerInfoCall;
        public IAsyncStreamWriter<PlayerInfoRequest> requestPlayerInfoStream;
        public IAsyncStreamReader<PlayerInfoResponse> responsePlayerInfoStream;

        private string clientId = "0000";
        private string clientIdToSend = "11111";

        private bool isValidMove = true;
        GameMessage gameMessage;
        OpponentGameData opponentGameData;
        OpponentUser opponentUser;
        User user;

        public Form1()
        {
            InitializeComponent();

            IPAddress[] localIP = Dns.GetHostAddresses(Dns.GetHostName());

            foreach (IPAddress address in localIP)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    ServerIPtextBox.Text = address.ToString();
                }
            }

            ChangeButtonsStatus(new Dictionary<string, bool>
            {
                {"btnTic",  false},
                {"Symbol" , false},
                {"SurrenderButton" , false},
                {"Send" , true},
                {"NewGameButton" , false},
            });
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                string address = $"http://{ServerIPtextBox.Text}:{ServerPortTextBox.Text}";
                channel = GrpcChannel.ForAddress("https://localhost:7069");
                client = new Greeter.GreeterClient(channel);

                streamingPlayerCall = client.SendPlayerGameData();
                requestPlayerStream = streamingPlayerCall.RequestStream;
                responsePlayerStream = streamingPlayerCall.ResponseStream;

                streamingCall = client.SendPlayerMessage();
                requestStream = streamingCall.RequestStream;
                responseStream = streamingCall.ResponseStream;

                streamingPlayerInfoCall = client.SendPlayerInfoData();
                requestPlayerInfoStream = streamingPlayerInfoCall.RequestStream;
                responsePlayerInfoStream = streamingPlayerInfoCall.ResponseStream;

                ChangeButtonsStatus(new Dictionary<string, bool> { ["Symbol"] = true });

                await SendGameDataRequestAsync(new PlayerGameDataRequest { ClientId = clientId, ClientIdToSend = clientIdToSend, FirstTime = true });
                await SendChatInfoAsync(new PlayerChatInfoRequest { ClientId = clientId, ClientIdToSend = clientIdToSend, FirstTime = true });
                await SendPlayerInfoAsync(new PlayerInfoRequest { ClientId = clientId, ClientIdToSend = clientIdToSend, FirstTime = true });

            }
            catch
            {
                MessageBox.Show("Error while starting the server. Please check the details entered and try again.");
            }
        }

        public async Task SendGameDataRequestAsync(PlayerGameDataRequest data)
        {
            Task.Run(async () =>
            {
                await foreach (var message in responsePlayerStream.ReadAllAsync())
                {
                    Invoke((Action)(() =>
                    {
                        opponentGameData = new OpponentGameData
                        {
                            Position = message.Position,
                            Text = message.Text,
                            ClientPlayed = message.ClientPlayed
                        };


                        ProcessGameMessage(opponentGameData);
                    }));
                }
            });

            await requestPlayerStream.WriteAsync(data);
        }

        public async Task SendChatInfoAsync(PlayerChatInfoRequest data)
        {
            Task.Run(async () =>
            {
                await foreach (var message in responseStream.ReadAllAsync()) 
                {                   
                    Invoke((Action)(() =>
                    {
                        ChatTextBox.Text += $"{opponentUser.Nickname + ": " + message.Message}\r\n";
                    }));
                }
            });

            await requestStream.WriteAsync(data);
        }

        public async Task SendPlayerInfoAsync(PlayerInfoRequest data)
        {
            Task.Run(async () =>
            {
                await foreach (var message in responsePlayerInfoStream.ReadAllAsync())
                {
                    Invoke((Action)(() =>
                    {
                        opponentUser = new OpponentUser
                        {
                            Nickname = message.Nickname,
                            ChosenSymbol = message.ChosenSymbol
                        };

                        if (!string.IsNullOrEmpty(opponentUser.ChosenSymbol))
                            DisableButton(FindButtonByName(string.Concat("Symbol", opponentUser.ChosenSymbol)));
                    }));
                }
            });          

            await requestPlayerInfoStream.WriteAsync(data);
        }        

        private async void SendButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(MessageTextBox6.Text))
            {
                await SendChatInfoAsync(new PlayerChatInfoRequest { ClientId = clientId, ClientIdToSend = clientIdToSend, Message = MessageTextBox6.Text, FirstTime = false });

                ChatTextBox.Text += $"{user.Nickname + ": " + MessageTextBox6.Text}\r\n";
                MessageTextBox6.Text = "";
            }
        }  

        private void ProcessGameMessage(OpponentGameData oponnentGameData)
        {
            switch (oponnentGameData.Position)
            {
                case "SurrenderButton":
                    ReceivedSurrenderUser();
                    break;
                case "NewGameButton":
                    ReceivedNewGameUser();
                    break;
                default:
                    SetBoardPosition(oponnentGameData);
                    break;
            }
        }       

        private void DisableButton(Button button)
        {
            button.Enabled = false;
        }

        private void ChangeButtonsStatus(Dictionary<string, bool> listItems)
        {

            foreach (KeyValuePair<string, bool> item in listItems)
            {
                foreach (Control control in Controls)
                {
                    if (control is Button button && button.Name.Contains(item.Key))
                        button.Enabled = item.Value;                    
                }
            }
        }

        private bool isDraw()
        {
            foreach (Control control in Controls)
            {
                if (control is Button button && button.Name.Contains("btnTic") && button.Text == "")        //Verifica ainda resta campos vazios (ainda há jogadas a se fazer)
                    return false;
            }
            return true;    // Em caso de empate 
        }

        private void ResetButtonsToNewGame(string text, string type)
        {
            foreach (Control control in Controls)
            {
                if (control is Button button && button.Name.Contains(type))
                {
                    button.BackColor = Color.White;
                    button.Text = text;
                }
            }
        }

        private void SetBoardPosition(OpponentGameData oponnentGameData)
        {
            Button button = FindButtonByName(oponnentGameData.Position);

            button.Text = opponentUser.ChosenSymbol;
            button.BackColor = Color.Orange;

            isValidMove = oponnentGameData.ClientPlayed;
            CheckScore();

            DisableButton(button);
        }

        private Button FindButtonByName(string position)
        {
            foreach (Control control in Controls)
            {
                if (control is Button button && button.Name == position)
                {
                    return button;
                }
            }
            return null;
        }

        private async void Symbol_Click(object sender, EventArgs e)
        {
            try
            {
                Button symbolButton = (Button)sender;

                if (symbolButton == SymbolX || symbolButton == SymbolO)
                {
                    user = new User
                    {
                        Nickname = NickName.Text,
                        ChosenSymbol = symbolButton.Text
                    };
                }

                if (ValidateUserNickName())
                {

                    ChangeButtonsStatus(new Dictionary <string, bool>
                    {
                        {  "btnTic", true },
                        { "SurrenderButton" , true },
                        { "Send" , true },
                        { "Symbol" , false },
                    });

                    await SendPlayerInfoAsync(new PlayerInfoRequest { ClientId = clientId, ClientIdToSend = clientIdToSend, Nickname = user.Nickname, ChosenSymbol = user.ChosenSymbol, FirstTime = false });
                }
            }
            catch
            {
                MessageBox.Show("Client not yet connected. Please wait.");
            }

        }

        private bool ValidateUserNickName()
        {
            if (string.IsNullOrEmpty(NickName.Text))
            {
                MessageBox.Show("Insert Nickname");
                return false;
            }
            return true;
        }

        void CheckScore()
        {
            List<int[]> winningCombinations = new List<int[]>
            {
                // Vitórias no tabuleiro 1
                new int[] { 1, 2, 3 },
                new int[] { 4, 5, 6 },
                new int[] { 7, 8, 9 },
                new int[] { 1, 4, 7 },
                new int[] { 2, 5, 8 },
                new int[] { 3, 6, 9 },
                new int[] { 1, 5, 9 },
                new int[] { 3, 5, 7 },

                // Vitórias no tabuleiro 2
                new int[] { 10, 11, 12 },
                new int[] { 13, 14, 15 },
                new int[] { 16, 17, 18 },
                new int[] { 10, 13, 16 },
                new int[] { 11, 14, 17 },
                new int[] { 12, 15, 18 },
                new int[] { 10, 14, 18 },
                new int[] { 12, 14, 16 },

                // Vitórias no tabuleiro 3
                new int[] { 19, 20, 21 },
                new int[] { 22, 23, 24 },
                new int[] { 25, 26, 27 },
                new int[] { 19, 22, 25 },
                new int[] { 20, 23, 26 },
                new int[] { 21, 24, 27 },
                new int[] { 19, 23, 27 },
                new int[] { 21, 23, 25 },

                // Vitórias verticais
                new int[] { 1, 10, 19 },
                new int[] { 2, 11, 20 },
                new int[] { 3, 12, 21 },
                new int[] { 4, 13, 22 },
                new int[] { 5, 14, 23 },
                new int[] { 6, 15, 24 },
                new int[] { 7, 16, 25 },
                new int[] { 8, 17, 26 },
                new int[] { 9, 18, 27 },

                // Vitórias diagonais
                new int[] { 1, 11, 21 },
                new int[] { 4, 14, 24 },
                new int[] { 7, 17, 27 },
                new int[] { 3, 11, 19 },
                new int[] { 6, 14, 22 },
                new int[] { 9, 17, 25 },
                new int[] { 1, 13, 25 },
                new int[] { 2, 14, 26 },
                new int[] { 3, 15, 27 },
                new int[] { 19, 13, 1 },
                new int[] { 20, 14, 2 },
                new int[] { 21, 15, 3 },
                new int[] { 1, 14, 27 },
                new int[] { 3, 14, 25 },
                new int[] { 19, 14, 9 },
                new int[] { 21, 14, 7 }
            };

            foreach (var combination in winningCombinations)
            {
                if (CheckIndex(combination[0], combination[1], combination[2]))
                {
                    break;
                }
                else if (isDraw())
                {
                    ChangeButtonsStatus(new Dictionary<string, bool>
                    {
                        { "btnTic" , false },
                        { "NewGameButton" , true },
                        { "SurrenderButton" , false },
                    });

                    MessageBox.Show($"EMPATE");
                }
            }
        }

        private bool CheckIndex(int index1, int index2, int index3)
        {
            Button btn1 = GetButtonByIndex(index1);
            Button btn2 = GetButtonByIndex(index2);
            Button btn3 = GetButtonByIndex(index3);

            bool winner = btn1.Text != "" && btn1.Text == btn2.Text && btn2.Text == btn3.Text;

            if (winner)
            {
                MarkWinnerBoard(btn1, btn2, btn3, isValidMove ? opponentUser.Nickname : user.Nickname);          //True = o adversário acabou de jogar; False = o player atual acabou de fazer uma jogada e chamou o método.
            }

            return winner;
        }

        private Button GetButtonByIndex(int index)
        {
            return this.Controls.Find("btnTic" + index.ToString(), true).FirstOrDefault() as Button;
        }

        private void MarkWinnerBoard(Button btn1, Button btn2, Button btn3, string winner)
        {
            btn1.BackColor = Color.Aqua;
            btn2.BackColor = Color.Aqua;
            btn3.BackColor = Color.Aqua;

            ChangeButtonsStatus(new Dictionary<string, bool>
            {
                { "btnTic" , false },
                { "NewGameButton" , true },
                { "SurrenderButton" , false }
            });

            MessageBox.Show($"The winner is Player {winner}", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void SurrenderButton_Click(object sender, EventArgs e)
        {
            gameMessage = new GameMessage
            {
                Position = "SurrenderButton"
            };

            ChangeButtonsStatus(new Dictionary<string, bool>
            {
                { "NewGameButton" , true },
                { "btnTic" , false },
                { "SurrenderButton" , false }
            });
            MessageBox.Show($"You surrendered", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await SendGameDataRequestAsync(new PlayerGameDataRequest
            {
                Position = gameMessage.Position,
                ClientId = clientId,
                ClientIdToSend = clientIdToSend,
                FirstTime = false
            });

            //SendMessageAsync($"The winner is Player {OpponentName}");
        }

        private async void ReceivedSurrenderUser()
        {
            ChangeButtonsStatus(new Dictionary<string, bool>
            {
                { "NewGameButton", true },
                { "btnTic", false },
                { "SurrenderButton", false },
            });
            MessageBox.Show($"Opponent surrendered", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //await SendMessageAsync($"The winner is Player {user.Nickname}");
        }

        private async void NewGameButton_Click(object sender, EventArgs e)
        {
            gameMessage = new GameMessage
            {
                Position = "NewGameButton"
            };

            await SendGameDataRequestAsync(new PlayerGameDataRequest
            {
                Position = gameMessage.Position,
                ClientId = clientId,
                ClientIdToSend = clientIdToSend,
                FirstTime = false
            });

            ChangeButtonsStatus(new Dictionary<string, bool>
            {
                { "btnTic" , true },
                { "NewGameButton" , false },
                { "SurrenderButton", true },
            });

            ResetButtonsToNewGame("", "btnTic");
            isValidMove = true;
        }

        private void ReceivedNewGameUser()
        {
            ChangeButtonsStatus(new Dictionary<string, bool>
            {
                { "btnTic", true },
                { "NewGameButton", false },
                { "SurrenderButton", true },
            });

            ResetButtonsToNewGame("", "btnTic");

            isValidMove = true;
        }

        private async void btnTic1_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic1);
        }

        private async void btnTic2_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic2);
        }

        private async void btnTic3_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic3);
        }

        private async void btnTic4_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic4);
        }

        private async void btnTic5_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic5);
        }

        private async void btnTic6_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic6);
        }

        private async void btnTic7_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic7);
        }

        private async void btnTic8_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic8);
        }

        private async void btnTic9_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic9);
        }

        private async void btnTic10_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic10);
        }

        private async void btnTic11_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic11);
        }

        private async void btnTic12_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic12);
        }

        private async void btnTic13_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic13);
        }

        private async void btnTic14_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic14);
        }

        private async void btnTic15_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic15);
        }

        private async void btnTic16_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic16);
        }

        private async void btnTic17_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic17);
        }

        private async void btnTic18_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic18);
        }

        private async void btnTic19_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic19);
        }

        private async void btnTic20_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic20);
        }

        private async void btnTic21_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic21);
        }

        private async void btnTic22_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic22);
        }

        private async void btnTic23_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic23);
        }

        private async void btnTic24_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic24);
        }

        private async void btnTic25_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic25);
        }

        private async void btnTic26_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic26);
        }

        private async void btnTic27_Click(object sender, EventArgs e)
        {
            await HandleButtonMove(btnTic27);
        }

        private async Task HandleButtonMove(Button button)
        {
            if (isValidMove)
            {
                button.Text = user.ChosenSymbol;
                button.BackColor = Color.PaleGreen;

                gameMessage = new GameMessage
                {
                    Position = button.Name,
                    Text = user.ChosenSymbol,
                    ClientPlayed = true
                };

                isValidMove = false;

                await SendGameDataRequestAsync(new PlayerGameDataRequest 
                { 
                    Position = gameMessage.Position,
                    Text = gameMessage.Text,
                    ClientPlayed = gameMessage.ClientPlayed,
                    ClientId = clientId, 
                    ClientIdToSend = clientIdToSend, 
                    FirstTime = false 
                });

                DisableButton(button);

                CheckScore();
            }
        }
    }
}