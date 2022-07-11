using BE.SignalR.Service;
using Microsoft.AspNetCore.SignalR;


namespace BE.SignalR.Hub
{

    public class SignalRChatHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IServiceProvider _serviceProvider;

        public SignalRChatHub(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public override Task OnConnectedAsync() //Faccio un override nel metodo OnConnectedAsync
        {
            try
            {
                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    ISignalRChatService scopedProcessingService =
                        scope.ServiceProvider.GetRequiredService<ISignalRChatService>(); //Prendo il service custom

                    //Processo la richiesta di connessione prendendo anche tutti i parametri
                    scopedProcessingService.OnConnect(
                        Context.ConnectionId, 
                        Context.GetHttpContext().Request.Query["ChatCode"], 
                        Context.GetHttpContext().Request.Query["UserName"]);
                }

            }
            catch (Exception ex)
            {
            }
            return Task.CompletedTask;
        }

    }
}
