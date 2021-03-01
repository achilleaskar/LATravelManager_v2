using LATravelManager.Model.BookingData;
using LATravelManager.Model.People;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LATravelManager.Model.Security
{
    public class LoginData : BaseModel
    {
        #region Fields

        private ObservableCollection<ChangeInBooking> _Changes;
        private User _LastEditedBy;
        private string _Link;
        private string _Name;
        private string _Password;
        private string _Username;

        #endregion Fields

        #region Properties

        [NotMapped]
        public string LoadedPassword { get; set; }

        public string EncryptedPassword { get; set; }

        public User LastEditedBy
        {
            get
            {
                return _LastEditedBy;
            }

            set
            {
                if (_LastEditedBy == value)
                {
                    return;
                }

                _LastEditedBy = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        public string Link
        {
            get
            {
                return _Link;
            }

            set
            {
                if (_Link == value)
                {
                    return;
                }

                _Link = value;
                RaisePropertyChanged();
            }
        }

        public DateTime? ModifiedDate { get; set; }

        [Required]
        [MaxLength(30)]
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
        [NotMapped]
        public string Password
        {
            get
            {
                return _Password;
            }

            set
            {
                if (_Password == value)
                {
                    return;
                }
                _Password = value;
                RaisePropertyChanged();
            }
        }

        [Required]
        [MaxLength(30)]
        public string Username
        {
            get
            {
                return _Username;
            }

            set
            {
                if (_Username == value)
                {
                    return;
                }

                _Username = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}