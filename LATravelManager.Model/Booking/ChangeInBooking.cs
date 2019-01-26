using LATravelManager.BaseTypes;
using System;

namespace LATravelManager.Models
{
    public class ChangeInBooking : BaseModel
    {
        #region Fields + Constructors

        private string _Description = string.Empty;

        private int _ItemId;

        private Type _Type;

        private User _User;

        public ChangeInBooking()
        {
        }

        public const string DescriptionPropertyName = nameof(Description);
        public const string ItemIdPropertyName = nameof(ItemId);
        public const string TypePropertyName = nameof(Type);

        public const string UserPropertyName = nameof(User);

        #endregion Fields + Constructors

        #region Properties

        /// <summary>
        /// Sets and gets the Description property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description == value)
                {
                    return;
                }
                _Description = value;
                RaisePropertyChanged(DescriptionPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the ItemId property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public int ItemId
        {
            get
            {
                return _ItemId;
            }

            set
            {
                if (_ItemId == value)
                {
                    return;
                }

                _ItemId = value;
                RaisePropertyChanged(ItemIdPropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the Type property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public Type Type
        {
            get
            {
                return _Type;
            }

            set
            {
                if (_Type == value)
                {
                    return;
                }

                _Type = value;
                RaisePropertyChanged(TypePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the User property.
        /// Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public User User
        {
            get
            {
                return _User;
            }

            set
            {
                if (_User == value)
                {
                    return;
                }

                _User = value;
                RaisePropertyChanged(UserPropertyName);
            }
        }

        #endregion Properties
    }
}