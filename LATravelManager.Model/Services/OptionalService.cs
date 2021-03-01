namespace LATravelManager.Model.Services
{
    public class OptionalService : Service
    {
        public OptionalService()
        {
            Title = "Προαιρετική";
        }

        public override string GetDescription()
        {
            return CompanyInfo;
        }
    }
}