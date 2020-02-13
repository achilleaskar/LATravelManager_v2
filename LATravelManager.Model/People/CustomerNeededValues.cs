using System;

namespace LATravelManager.Model.People
{
    public class CustomerNeededValues
    {
        public string Name { get; set; }
        public string Surename { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public decimal Price { get; set; }
        public string StartPlace { get; set; }
        public string ReturnPlace { get; set; }
        public string Tel { get; set; }
    }
}