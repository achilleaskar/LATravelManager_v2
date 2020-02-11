using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model.Hotels
{
    public class HotelOptions
    {
        public Hotel Hotel { get; set; }
        public int Counter { get; set; }
        public DateTime Date { get; set; }

        public List<Option> Options { get; set; }
    }
}
