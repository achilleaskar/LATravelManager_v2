namespace LATravelManager.Model.Locations
{
    public class Airport
    {
        public Airport(string name)
        {
            Name = name;
        }

        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}