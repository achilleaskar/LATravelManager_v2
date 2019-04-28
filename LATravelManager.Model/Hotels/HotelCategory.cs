using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Hotels
{
    public class HotelCategory : BaseModel
    {
        #region Constructors

        public HotelCategory()
        {
        }

        public List<Hotel> Hotels { get; set; }

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