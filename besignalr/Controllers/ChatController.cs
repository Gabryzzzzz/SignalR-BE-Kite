#pragma warning disable CS8602 // Dereferenziamento di un possibile riferimento Null.
using BE.SignalR.Hub;
using BE.SignalR.StaticData;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BE.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class ChatController : ControllerBase
{

    private readonly ILogger<ChatController> _logger;
    private readonly IHubContext<SignalRChatHub> _hubContext;

    public ChatController(IHubContext<SignalRChatHub> hubContext, ILogger<ChatController> logger)
    {
        _hubContext = hubContext;
        _logger = logger;
    }

    public class SendMessageRequest
    {
        public string sender { get; set; } = string.Empty;
        public string chat_code { get; set; } = string.Empty;
        public string msg { get; set; } = string.Empty;
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(SendMessageRequest request)
    {
        /*
         * 
         */
        await SendsignalrMessage(request.sender, request.chat_code, request.msg);
        return new ObjectResult(true);

    }

    [HttpGet]
    public IActionResult GetMessages([FromQuery]string chat_code)
    {
        /*5 m 
         * Questo endpoint ha il solo scopo di mandare tutti i messaggi della chat al momento dell'avvio del FE,
         * 
         * Una buona alternativa sarebbe anche mandare tutti i messaggi vecchi attraverso l'hub appena creata
         */
        return new ObjectResult(db.chats.FirstOrDefault(x => x.chat_code == chat_code).messages);

    }

    private async Task SendsignalrMessage(string sender, string chat_code, string message)
    {
        List<chat> chats = new List<chat>();

        chat chat = db.chats.First(x => x.chat_code == chat_code);

        message message_to_send = new message
        {
            chat_code = chat_code,
            msg = message,
            sender = sender,
            timestamp = DateTime.Now
        };

        chat.messages.Add(message_to_send);

        foreach (user item in chat.users)
        {
            await _hubContext.Clients.Clients(item.connectionId).SendAsync("message", message_to_send);
        }
    }

}

#pragma warning restore CS8602 // Dereferenziamento di un possibile riferimento Null.