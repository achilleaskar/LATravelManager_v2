using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class Partner : EditTracker, INamed
    {
        public string Emails { get; set; }

        #region Properties

        [Required]
        [StringLength(20,MinimumLength =3)]
        public string Name { get; set; }

        public string Note { get; set; }

        [Required]
        [StringLength(20)]
        [Phone]
        public string Tel { get; set; }

        #endregion Properties
    }
}