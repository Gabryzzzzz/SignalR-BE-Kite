using BE.SignalR.StaticData;

namespace BE.SignalR.Service
{

    public interface ISignalRChatService
    {
        Task OnConnect(string connectionsId, string connectionName, string UserName);
    }

    public class SignalRChatService : ISignalRChatService
    {

        public SignalRChatService()
        {

        }

        public async Task OnConnect(string connectionsId, string connectionName, string UserName)
        {
            if (connectionName != null) //Controllo se non è arrivata una richiesta sporca
            {
                var chat = db.chats.FirstOrDefault(x => x.chat_code == connectionName); //Cerco la chat con il code == connectioName
                user user = new user() { connectionId = connectionsId, username = UserName }; //Creo un nuovo utente

                if (chat == null) //Se la chat non esiste
                {
                    List<user> users = new List<user>();    //Inizializzo una nuova lista di utenti
                    users.Add(user);                        //Ci aggiungo l'utente che chiede connessione
                    db.chats.Add(new chat()                 //Inizializzo una nuova chat con il chat_code e la lista utenti
                    {
                        chat_code = connectionName,
                        users = users
                    });
                }
                else
                {
                    user user_find = chat.users.FirstOrDefault(x => x.username == UserName);    //Se la chat esiste controllo se Quell'utente
                    if (user_find != null)                                                      //si è precedentemente connesso
                    {
                        chat.users.First(x => x.username == UserName).connectionId = connectionsId; //Se trovo un utente aggiorno il nuovo connection id
                    }
                    else
                    {
                        chat.users.Add(user); //Altrimenti aggiungo il nuovo utente alla chat
                    }
                }

            }
        }

        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
