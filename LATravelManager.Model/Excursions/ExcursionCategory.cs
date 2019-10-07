﻿using System.ComponentModel.DataAnnotations;

namespace LATravelManager.Model.Excursions
{
    public class ExcursionCategory : BaseModel
    {
        #region Constructors

        public ExcursionCategory()
        {
        }

        #endregion Constructors

        #region Fields

        private ExcursionTypeEnum _Category;
        private int _IndexNum;
        private string _Name = string.Empty;

        #endregion Fields

        #region Properties

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