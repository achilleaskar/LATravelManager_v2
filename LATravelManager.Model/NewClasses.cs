using System;
using System.ComponentModel.DataAnnotations;
using LATravelManager.Model.People;

namespace LATravelManager.Model
{
    public class PartnerInfo : BaseModel
    {
        public decimal Commision { get; set; }
        public decimal NetPrice { get; set; }
        public Partner Partner { get; set; }

        [StringLength(60)]
        public string PartnerEmail { get; set; }

        public bool ProformaSent { get; set; }
    }

    public class DisabledInfo : BaseModel
    {
        public DateTime? DisableDate { get; set; }
        public User DisabledBy { get; set; }
        public string CancelReason { get; set; }
    }
}