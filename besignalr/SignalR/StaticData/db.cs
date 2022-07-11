namespace BE.SignalR.StaticData
{

    public class message
    {
        public string chat_code { get; set; } = string.Empty;
        public string sender { get; set; } = string.Empty;
        public string msg { get; set; } = string.Empty;
        public DateTime timestamp { get; set; } = DateTime.Now;
    }

    public class user
    {
        public string connectionId { get; set; }
        public string username { get; set; }
    }

    public class chat
    {
        public string chat_code { get; set; } = string.Empty;
        public List<user> users { get; set; } = new List<user>();
        public List<message> messages { get; set; } = new List<message>();
    }

    public static class db
    {
        public static List<chat> chats { get; set; } = new List<chat>();

    }

    /*
     * Per comodità simulo un database in classi statiche
     */
}
