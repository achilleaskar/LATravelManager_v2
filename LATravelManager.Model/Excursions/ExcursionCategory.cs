using LATravelManager.BaseTypes;
using System.ComponentModel.DataAnnotations;
using static LATravelManager.Model.Enums;

namespace LATravelManager.Models
{
    public class ExcursionCategory : BaseModel
    {
        #region Constructors

        public ExcursionCategory()
        {
            Tittle = "Ο τύπος εκδρομής";
        }

        #endregion Constructors

        #region Fields


        private ExcursionTypeEnum _Category;
        private int _IndexNum;
        private string _Name = string.Empty;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the Category property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        [Required]
        public ExcursionTypeEnum Category
        {
            get
            {
                return _Category;
            }

            set
            {
                if (_Category == value)
                {
                    return;
                }

                _Category = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the IndexNum property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public int IndexNum
        {
            get
            {
                return _IndexNum;
            }

            set
            {
                if (_IndexNum == value)
                {
                    return;
                }

                _IndexNum = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the Name property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>

        [StringLength(30)]
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

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        #endregion Methods
    }


}