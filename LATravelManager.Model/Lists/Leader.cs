using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Lists
{
    public class Leader : BaseModel, INamed
    {

        #region Fields

        private string _Name = string.Empty;
        private string _Tel;

        #endregion Fields

        #region Properties

        [Required]
        [StringLength(20)]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {
                if (_Name == value)
                {
                    return;
                }

                _Name = value;
                RaisePropertyChanged();
            }
        }
        
        [Required]
        [MaxLength(15), MinLength(10)]
        [Phone]
        public string Tel
        {
            get
            {
                return _Tel;
            }

            set
            {
                if (_Tel == value)
                {
                    return;
                }

                _Tel = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

    }
}