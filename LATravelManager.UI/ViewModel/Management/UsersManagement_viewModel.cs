using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using LaTravelManager.BaseTypes;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Security;
using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LaTravelManager.ViewModel.Management
{
    public class UsersManagement_viewModel : AddEditBase<UserWrapper, User>
    {
        #region Constructors

        public UsersManagement_viewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Χρηστών";
            PassWord = new SecureString();
            PasswordRepeat = new SecureString();
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<StartingPlace> _BaseLocations;

        private SecureString _PassWord;

        private SecureString _PasswordRepeat;

        private ObservableCollection<User> _Users;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Sets and gets the BaseLocations property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<StartingPlace> BaseLocations
        {
            get
            {
                return _BaseLocations;
            }

            set
            {
                if (_BaseLocations == value)
                {
                    return;
                }

                _BaseLocations = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the PassWord property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public SecureString PassWord
        {
            get
            {
                return _PassWord;
            }

            set
            {
                if (_PassWord == value)
                {
                    return;
                }

                _PassWord = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Sets and gets the PasswordRepeat property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public SecureString PasswordRepeat
        {
            get
            {
                return _PasswordRepeat;
            }

            set
            {
                if (_PasswordRepeat == value)
                {
                    return;
                }
                _PasswordRepeat = value;
                if (ArePasswordsOk())
                {
                    SelectedEntity.HashedPassword = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PassWord));
                }
                RaisePropertyChanged();
            }
        }

        public override bool CanAddEntity()
        {
            return base.CanAddEntity() && ArePasswordsOk();
        }

        /// <summary>
        /// Sets and gets the Users property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<User> Users
        {
            get
            {
                return _Users;
            }

            set
            {
                if (_Users == value)
                {
                    return;
                }

                _Users = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private bool ArePasswordsOk()
        {
            if (PassWord != null && PasswordRepeat != null)
            {
                byte[] enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PassWord));
                byte[] enteredValueHash2 = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PasswordRepeat));

                return PasswordHashing.SequenceEquals(enteredValueHash, enteredValueHash2);
            }
            return false;
        }

        public override bool CanSaveChanges()
        {
            if (SelectedEntity == null)
            {
                SelectedEntity = new UserWrapper();
            }

            return BasicDataManager.HasChanges() && BasicDataManager.IsContextAvailable;
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            MainCollection = new ObservableCollection<UserWrapper>((BasicDataManager.Users).Select(u => new UserWrapper(u)));
            BaseLocations = new ObservableCollection<StartingPlace>(BasicDataManager.StartingPlaces);
        }

        #endregion Methods
    }
}