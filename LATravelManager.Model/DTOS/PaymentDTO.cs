using LATravelManager.Model.People;
using System;

namespace LATravelManager.Model.DTOS
{
    public class PaymentDTO
    {
        public PaymentDTO()
        {

        }
        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public bool Outgoing { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public User User { get; set; }
    }
}