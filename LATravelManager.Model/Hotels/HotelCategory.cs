using LATravelManager.BaseTypes;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class HotelCategory : BaseModel
    {
        #region Constructors

        public HotelCategory()
        {
            Tittle = "Η Κατηγορία Ξενοδοχείου";
        }

        #endregion Constructors

        #region Properties

        [Required]
        [StringLength(15)]
        public string Name { get; set; }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }
}