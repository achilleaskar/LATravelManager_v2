namespace LATravelManager.Model.Services
{
    public class FerryService : Service
    {
        public FerryService()
        {
            Title = "Ακτοπλοϊκό";
        }

       

        public override string GetDescription()
        {
            return $"{Title} από {From ?? ""} για {To ?? ""}";
        }
    }
}