using LATravelManager.Model;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Models
{
    public class HotelCategory : BaseModel
    {
        #region Constructors

        public HotelCategory()
        {
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