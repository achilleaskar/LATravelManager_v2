using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model
{
    public class Airline : BaseModel
    {
        public int Checkin { get; set; }
        public string Name { get; set; }
    }
}
