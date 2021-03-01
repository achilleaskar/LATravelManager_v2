namespace LATravelManager.Model
{
    public class Airline : BaseModel,INamed
    {
        public int Checkin { get; set; }
        public string Name { get; set; }
    }
}