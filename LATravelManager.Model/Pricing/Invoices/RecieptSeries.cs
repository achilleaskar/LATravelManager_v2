using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Pricing.Invoices
{
    public class RecieptSeries : BaseModel, INamed
    {

        #region Properties

        [ConcurrencyCheck]
        public int CurrentNumber { get; set; }

        public DateTime? DateEnded { get; set; }

        public DateTime DateStarted { get; set; }

        public bool Disabled { get; set; }

        [MaxLength(20)]
        public string Letter { get; set; }

        [Index(IsUnique = true)]
        [MaxLength(40)]
        public string SerieCode { get; set; }

        [MaxLength(40)]
        public string Name
        { 
            get; 
            set; }

        public HashSet<Reciept> Reciepts { get; set; }

        public RecieptTypeEnum RecieptType { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name + $" ({ DateStarted.ToString("dd/MM/yy")}-{(DateEnded.HasValue ? DateEnded.Value.ToString("dd/MM/yy") : "σήμερα")})" + (Disabled ? "Ολοκληρωμένη" : "");
        }

        #endregion Methods
    }
}