using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Services
{
    public class Service : EditTracker
    {
        [NotMapped]
        public string Tittle { get; set; }
    }
}