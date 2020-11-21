using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model.DTOS
{
    public class CustomerDTO
    {
        public CustomerDTO()
        {

        }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public string StartingPlace { get; set; }
        public string Name { get; set; }
        public string Surename { get; set; }
        public string Tel { get; set; }
        public decimal Price { get; set; }
    }
}
