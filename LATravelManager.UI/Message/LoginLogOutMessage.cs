namespace LATravelManager.UI.Message
{
    public class LoginLogOutMessage
    {
        public LoginLogOutMessage(bool login)
        {
            Login = login;
        }

        public bool Login { get; }
    }
}