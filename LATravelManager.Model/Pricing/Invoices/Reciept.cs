using LATravelManager.Model.People;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Pricing.Invoices
{
    public class Reciept : BaseModel
    {
        public Company Company { get; set; }

        public int? CompanyId { get; set; }

        [Required]
        public RecieptTypeEnum RecieptType { get; set; }

        [Required]
        public RecieptSeries Series { get; set; }

        [Required]
        public int RecieptNumber { get; set; }

        public string RecieptDescription { get; set; }

        public CustomFile RecieptFile { get; set; }

        public int? RecieptFileId { get; set; }

        public bool Canceled { get; set; }
    }
}