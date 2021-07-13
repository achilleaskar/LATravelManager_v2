using System;

namespace LATravelManager.Model.DTOS
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {
        }

        public int Id { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string StartingPlace { get; set; }
        public string ReturningPlace { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Tel { get; set; }
        public decimal Price { get; set; }
        public int CustomerHasBusIndex { get; set; }

        public int CustomerHasPlaneIndex { get; set; }

        public int CustomerHasShipIndex { get; set; }
    }
}