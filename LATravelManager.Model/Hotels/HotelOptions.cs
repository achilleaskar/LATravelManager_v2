using System;
using System.Collections.Generic;

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