using ClientOne.DTO;
using ClientOne.DTO.Interface;
using ClientOne.Entity;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;

namespace ClientOne
{
    public partial class Form1 : Form
    {
        private GrpcChannel channel;
        private Greeter.GreeterClient client1;

        public AsyncDuplexStreamingCall<PlayerChatInfoRequest, PlayerChatInfoResponse> streamingCall;
        public IAsyncStreamWriter<PlayerChatInfoRequest> requestStream;
        public IAsyncStreamReader<PlayerChatInfoResponse> responseStream;

        public AsyncDuplexStreamingCall<PlayerGameDataRequest, PlayerGameDataResponse> streamingPlayerCall;
        public IAsyncStreamWriter<PlayerGameDataRequest> requestPlayerStream;
        public IAsyncStreamReader<PlayerGameDataResponse> responsePlayerStream;

        public AsyncDuplexStreamingCall<PlayerInfoRequest, PlayerInfoResponse> streamingPlayerInfoCall;
        public IAsyncStreamWriter<PlayerInfoRequest> requestPlayerInfoStream;
        public IAsyncStreamReader<PlayerInfoResponse> responsePlayerInfoStream;

        private string clientId = "11111";
        private string clientIdToSend = "0000";

        private Socket client;
        public StreamReader STR;
        public StreamWriter STW;
        public string recieve;
        public string TextToSend;
        private bool isValidMove = true;
        Button symbolButton;
        GameMessage gameMessage;
        OponnentGameData oponnentGameData;
        User user;
        string OpponentName = null, OpponentSymbol = null;
        Dictionary<string, bool> statusChangeItems;

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
                    

            statusChangeItems = new Dictionary<string, bool>
            {
                {"btnTic",  false},
                {"Symbol" , false},
                {"SurrenderButton" , false},
                {"Send" , true},
                {"NewGameButton" , false},
            };

            ChangeButtonsStatus(statusChangeItems);
        }

        private async void StartButton_Click(object sender, EventArgs e)
        {
            try
            {
                string address = $"http://{ServerIPtextBox.Text}:{ServerPortTextBox.Text}";

                channel = GrpcChannel.ForAddress("https://localhost:7069");

                client1 = new Greeter.GreeterClient(channel);

                streamingPlayerCall = client1.SendPlayerGameData();
                requestPlayerStream = streamingPlayerCall.RequestStream;
                responsePlayerStream = streamingPlayerCall.ResponseStream;

                streamingCall = client1.SendPlayerMessage();
                requestStream = streamingCall.RequestStream;
                responseStream = streamingCall.ResponseStream;

                streamingPlayerInfoCall = client1.SendPlayerInfoData();
                requestPlayerInfoStream = streamingPlayerInfoCall.RequestStream;
                responsePlayerInfoStream = streamingPlayerInfoCall.ResponseStream;
                ChangeButtonsStatus(statusChangeItems = new Dictionary<string, bool> { { "Symbol", true } });


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
                        oponnentGameData = new OponnentGameData
                        {
                            Position = message.Position,
                            Text = message.Text,
                            ClientPlayed = message.ClientPlayed
                        };


                        ProcessGameMessage(oponnentGameData);
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
                        ChatTextBox.Text += $"{OpponentName + ": " + message.Message}\r\n";
                    }));
                }
            });

            await requestStream.WriteAsync(data);
        }

        public async Task SendPlayerInfoAsync(PlayerInfoRequest data)
        {
            try
            {
                Task.Run(async () =>
                {
                    await foreach (var message in responsePlayerInfoStream.ReadAllAsync())
                    {
                        Invoke((Action)(() =>
                        {
                            OpponentName = message.Nickname;
                            OpponentSymbol = message.ChosenSymbol;

                            if (!string.IsNullOrEmpty(OpponentSymbol))
                                DisableButton(FindButtonByName(string.Concat("Symbol", OpponentSymbol)));
                        }));
                    }
                });

                await requestPlayerInfoStream.WriteAsync(data);
            }
            catch (Exception e)
            {
                // Lidar com exceções, se necessário
            }
        }



        private async Task StartServerAsync(IPAddress ip, int port)
        {
            try
            {
                try
                {
                    Socket listenerSocket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    listenerSocket.Bind(new IPEndPoint(ip, port));
                    listenerSocket.Listen(1);

                    ChangeButtonsStatus(statusChangeItems = new Dictionary<string, bool> { { "Symbol", true } });

                    client = await listenerSocket.AcceptAsync();

                    ChatTextBox.AppendText("Connect to ClientTwo" + Environment.NewLine);
                    STR = new StreamReader(new NetworkStream(client));
                    STW = new StreamWriter(new NetworkStream(client));
                    STW.AutoFlush = true;

                    await ProcessReceivedMessagesAsync();
                }
                catch
                {
                    client = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    IPEndPoint endPoint = new IPEndPoint(ip, port);
                    await client.ConnectAsync(endPoint);

                    ChangeButtonsStatus(statusChangeItems = new Dictionary<string, bool> { { "Symbol", true } });

                    ChatTextBox.AppendText("Connect to ClientTwo" + Environment.NewLine);
                    STR = new StreamReader(new NetworkStream(client));
                    STW = new StreamWriter(new NetworkStream(client));
                    STW.AutoFlush = true;

                    await ProcessReceivedMessagesAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during server initialization: " + ex.Message);
            }
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

        private async Task SendMessageAsync(string message)
        {
            try
            {
                await STW.WriteLineAsync($"{message}");
                this.ChatTextBox.Invoke(new MethodInvoker(delegate ()
                {
                    ChatTextBox.AppendText(user.Nickname + ": " + message + Environment.NewLine);
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during sending message: " + ex.Message);
            }
        }

        private async Task ProcessReceivedMessagesAsync()
        {
            while (client.Connected)
            {
                try
                {
                    recieve = await STR.ReadLineAsync();

                    if (IsValidateJson(recieve))
                    {
                        ProcessReceivedJsonMessage(recieve);
                    }
                    else
                    {
                        ProcessReceivedNonJsonMessage(recieve);
                    }

                }
                catch (IOException ex)
                {
                    HandleDisconnection();
                    break;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }

        private void ProcessReceivedJsonMessage(string jsonMessage)
        {
            JObject receivedJson = JObject.Parse(recieve);
            string messageType = receivedJson["MessageType"].ToString();

            switch (messageType)
            {
                //case "GameMessage":
                //    ProcessGameMessage(receivedJson);
                //    break;
                case "UserMessage":
                    ProcessUserMessage(receivedJson);
                    break;
            }
        }

        private void ProcessReceivedNonJsonMessage(string message)
        {
            this.ChatTextBox.Invoke(new MethodInvoker(delegate ()
            {
                ChatTextBox.AppendText(OpponentName + ": " + message + Environment.NewLine);
            }));
            recieve = "";
        }

        private void ProcessGameMessage(OponnentGameData oponnentGameData)
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

        private void ProcessUserMessage(JObject jsonUserMessage)
        {
            User receiceMessage = JsonConvert.DeserializeObject<User>(recieve);

            OpponentName = receiceMessage.Nickname;
            OpponentSymbol = receiceMessage.ChosenSymbol;

            DisableButton(FindButtonByName(string.Concat("Symbol", OpponentSymbol)));
        }

        private bool IsValidateJson(string input)
        {
            try
            {
                JToken.Parse(recieve);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void HandleDisconnection()
        {
            try
            {
                // Fechar os fluxos e o socket
                STR.Close();
                STW.Close();
                client.Close();

                isValidMove = false;

                statusChangeItems = new Dictionary<string, bool>
                {
                    { "btnTic", false },
                    { "Symbol" , false },
                    { "SurrenderButton" , false},
                    { "Send" , false},
                    { "NewGameButton" , false}
                };

                ChangeButtonsStatus(statusChangeItems);
                ResetButtonsToNewGame("", "btnTic");

                MessageBox.Show("Disconnected from the server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during disconnection: " + ex.Message);
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

        private void SetBoardPosition(OponnentGameData oponnentGameData)
        {
            Button button = FindButtonByName(oponnentGameData.Position);

            button.Text = OpponentSymbol;
            button.BackColor = Color.PaleGreen;

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

        private async Task SendMoveAsync(IMessageType message)
        {
            string jsonMessage = null;

            try
            {
                if (message.MessageType == "GameMessage")
                {
                    jsonMessage = JsonConvert.SerializeObject(gameMessage);
                }
                else if (message.MessageType == "UserMessage")
                {
                    jsonMessage = JsonConvert.SerializeObject(user);
                }

                await STW.WriteLineAsync(jsonMessage);
                await STW.FlushAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error during sending message: " + ex.Message);
            }
        }

        private async void Symbol_Click(object sender, EventArgs e)
        {
            try
            {
                symbolButton = (Button)sender;

                if (symbolButton == SymbolX || symbolButton == SymbolO)
                {
                    user = new User
                    {
                        Nickname = NickName.Text,
                        ChosenSymbol = symbolButton.Text
                    };
                }                

                if (ValidateUserNickName() && ValidateChosenSymbol())
                {
                    statusChangeItems = new Dictionary<string, bool>
                {
                    {  "btnTic", true },
                    { "SurrenderButton" , true },
                    { "Send" , true },
                    { "Symbol" , false },
                };

                    ChangeButtonsStatus(statusChangeItems);

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

        private bool ValidateChosenSymbol()
        {
            if (string.IsNullOrEmpty(symbolButton.Text))
            {
                MessageBox.Show("Pick a symbol");
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
                    statusChangeItems = new Dictionary<string, bool>
                    {
                        { "btnTic" , false },
                        { "NewGameButton" , true },
                        { "SurrenderButton" , false },
                    };
                    ChangeButtonsStatus(statusChangeItems);

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
                MarkWinnerBoard(btn1, btn2, btn3, isValidMove ? OpponentName : user.Nickname);          //True = o adversário acabou de jogar; False = o player atual acabou de fazer uma jogada e chamou o método.
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

            statusChangeItems = new Dictionary<string, bool>
            {
                { "btnTic" , false },
                { "NewGameButton" , true },
                { "SurrenderButton" , false },
            };

            ChangeButtonsStatus(statusChangeItems);

            MessageBox.Show($"The winner is Player {winner}", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void SurrenderButton_Click(object sender, EventArgs e)
        {
            gameMessage = new GameMessage
            {
                Position = "SurrenderButton"
            };

            await SendGameDataRequestAsync(new PlayerGameDataRequest
            {
                Position = gameMessage.Position,
                ClientId = clientId,
                ClientIdToSend = clientIdToSend,
                FirstTime = false
            });

            statusChangeItems = new Dictionary<string, bool>
            {
                { "NewGameButton" , true },
                { "btnTic" , false },
                { "SurrenderButton" , false },
            };

            ChangeButtonsStatus(statusChangeItems);
            MessageBox.Show($"You surrendered", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //SendMessageAsync($"The winner is Player {OpponentName}");
        }

        private async void ReceivedSurrenderUser()
        {
            statusChangeItems = new Dictionary<string, bool>
            {
                { "NewGameButton", true },
                { "btnTic", false },
                { "SurrenderButton", false },
            };

            ChangeButtonsStatus(statusChangeItems);
            MessageBox.Show($"Opponent surrendered", "TicTacToe", MessageBoxButtons.OK, MessageBoxIcon.Information);

            await SendMessageAsync($"The winner is Player {user.Nickname}");
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

            statusChangeItems = new Dictionary<string, bool>
            {
                { "btnTic" , true },
                { "NewGameButton" , false },
                { "SurrenderButton", true },
            };

            ChangeButtonsStatus(statusChangeItems);
            ResetButtonsToNewGame("", "btnTic");

            isValidMove = true;
        }

        private void ReceivedNewGameUser()
        {
            statusChangeItems = new Dictionary<string, bool>
            {
                { "btnTic", true },
                { "NewGameButton", false },
                { "SurrenderButton", true },
            };

            ChangeButtonsStatus(statusChangeItems);
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
                button.BackColor = Color.Orange;

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