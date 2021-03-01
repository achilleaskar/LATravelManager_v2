namespace LATravelManager.Model.Services
{
    public class TransferService : Service
    {
        public TransferService()
        {
            Title = "Transfer";
        }

        public override string GetDescription()
        {
            return $"Transfer{(!string.IsNullOrWhiteSpace(From) ? " από " + From : "")+" " + (!string.IsNullOrWhiteSpace(To) ? " για " + To : "")}";

        }
    }
}