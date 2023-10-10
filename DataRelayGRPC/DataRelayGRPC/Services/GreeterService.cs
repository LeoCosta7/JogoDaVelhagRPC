using DataRelayGRPC;
using Grpc.Core;

namespace DataRelayGRPC.Services
{
    public class GreeterService : Greeter.GreeterBase
    {
        private static Dictionary<string, IServerStreamWriter<PlayerChatInfoResponse>> connectedClientsChat = new Dictionary<string, IServerStreamWriter<PlayerChatInfoResponse>>();
        private static Dictionary<string, IServerStreamWriter<PlayerGameDataResponse>> connectedPlayersGameData = new Dictionary<string, IServerStreamWriter<PlayerGameDataResponse>>();
        private static Dictionary<string, IServerStreamWriter<PlayerInfoResponse>> connectedPlayersInfo = new Dictionary<string, IServerStreamWriter<PlayerInfoResponse>>();

        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override async Task SendPlayerGameData(IAsyncStreamReader<PlayerGameDataRequest> request, IServerStreamWriter<PlayerGameDataResponse> responseStream, ServerCallContext context)
        {
            string clientIdAux = "";

            try
            {
                await foreach (var msg in request.ReadAllAsync())
                {
                    connectedPlayersGameData[msg.ClientId] = responseStream;
                    clientIdAux = msg.ClientId;

                    if (connectedPlayersGameData.TryGetValue(msg.ClientIdToSend, out var recipientStreamObject) && msg.FirstTime != true)
                    {
                        if (recipientStreamObject is IServerStreamWriter<PlayerGameDataResponse> recipientStream)
                            await recipientStream.WriteAsync(new PlayerGameDataResponse { Position = msg.Position, Text = msg.Text, ClientPlayed = msg.ClientPlayed });
                    }
                }
            }
            catch
            {
                if (connectedPlayersGameData.ContainsKey(clientIdAux))
                    connectedPlayersGameData.Remove(clientIdAux);
            }
        }

        public override async Task SendPlayerMessage(IAsyncStreamReader<PlayerChatInfoRequest> requestStream, IServerStreamWriter<PlayerChatInfoResponse> responseStream, ServerCallContext context)
        {
            string clientIdAux = "";
            try
            {
                await foreach (var clientInfo in requestStream.ReadAllAsync())
                {
                    connectedClientsChat[clientInfo.ClientId] = responseStream;
                    clientIdAux = clientInfo.ClientId;

                    if (connectedClientsChat.TryGetValue(clientInfo.ClientIdToSend, out var recipientStreamObject) && clientInfo.FirstTime != true)
                    {
                        if (recipientStreamObject is IServerStreamWriter<PlayerChatInfoResponse> recipientStreamForChat)
                            await recipientStreamForChat.WriteAsync(new PlayerChatInfoResponse { Message = clientInfo.Message });
                    }
                    else
                        await responseStream.WriteAsync(new PlayerChatInfoResponse { Message = "Conectado" });
                }
            }
            catch
            {
                if (connectedPlayersInfo.ContainsKey(clientIdAux))
                    connectedPlayersInfo.Remove(clientIdAux);
            }
        }

        public override async Task SendPlayerInfoData(IAsyncStreamReader<PlayerInfoRequest> requestStream, IServerStreamWriter<PlayerInfoResponse> responseStream, ServerCallContext context)
        {
            string clientIdAux = "";

            try
            {
                await foreach (var msg in requestStream.ReadAllAsync())
                {
                    connectedPlayersInfo[msg.ClientId] = responseStream;

                    clientIdAux = msg.ClientId;

                    if (connectedPlayersInfo.TryGetValue(msg.ClientIdToSend, out var recipientStreamObject) && msg.FirstTime != true)
                    {
                        if (recipientStreamObject is IServerStreamWriter<PlayerInfoResponse> recipientStream)
                            await recipientStream.WriteAsync(new PlayerInfoResponse { Nickname = msg.Nickname, ChosenSymbol = msg.ChosenSymbol });
                    }                        
                }
            }
            catch
            {
                RemoveDisconnectedClient(clientIdAux, connectedPlayersInfo);
            }
        }

        private void RemoveDisconnectedClient<T>(string clientId, Dictionary<string, T> dictionary)
        {
            if (dictionary.ContainsKey(clientId))
                dictionary.Remove(clientId);
        }
    }
}