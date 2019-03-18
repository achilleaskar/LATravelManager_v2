using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Models;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.Security;
using LATravelManager.UI.ViewModel.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Window_ViewModels
{
    public class LoginViewModel : MyViewModelBase
    {
        private readonly GenericRepository startingRepository;

        public LoginViewModel(GenericRepository startingRepository)
        {
            this.startingRepository = startingRepository;
            LoginCommand = new RelayCommand(async () => { await TryLogin(); }, CanLogin);
            PossibleUser = new User();
        }

        public NotifyTaskCompletion<int> LoginStatus { get; private set; }
        private string _ErrorMessage;
        private bool _LoginFailed;
        private SecureString _PasswordSecureString;
        private User _PossibleUser;

        public List<User> Users { get; set; }

        public string ErrorMessage
        {
            get
            {
                return _ErrorMessage;
            }

            set
            {
                if (_ErrorMessage == value)
                {
                    return;
                }

                _ErrorMessage = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand LoginCommand { get; set; }

        public bool LoginFailed
        {
            get
            {
                return _LoginFailed;
            }

            set
            {
                if (_LoginFailed == value)
                {
                    return;
                }

                _LoginFailed = value;
                RaisePropertyChanged();
            }
        }

        public SecureString PasswordSecureString
        {
            get
            {
                return _PasswordSecureString;
            }

            set
            {
                if (_PasswordSecureString == value)
                {
                    return;
                }

                _PasswordSecureString = value;
                RaisePropertyChanged();
            }
        }

        public User PossibleUser
        {
            get
            {
                return _PossibleUser;
            }

            set
            {
                if (_PossibleUser == value)
                {
                    return;
                }

                _PossibleUser = value;
                RaisePropertyChanged();
            }
        }

        private bool CanLogin()
        {
            //Execution should only be possible if both Username and Password have been supplied
            if (!string.IsNullOrWhiteSpace(PossibleUser.UserName) && PasswordSecureString != null && PasswordSecureString.Length > 0)
                return true;
            return false;
        }

        async private Task TryLogin()
        {
            try
            {
                //Search for the existance of the specified username, otherwise
                //set the relevant error message if the user is not found.
                Mouse.OverrideCursor = Cursors.Wait;
                User userFound = null;
                ErrorMessage = "Παρακαλώ περιμένετε...";

                userFound = await startingRepository.FindUserAsync(PossibleUser.UserName.ToLower());
                if (userFound == null)
                {
                    ErrorMessage = "Δεν βρέθηκε χρηστης.";
                    return;
                }

                //User exists. Check if specified password matches the actual
                //password for this user stored in the database

                //Get the Hash of the entered data
                byte[] enteredValueHash = null;
                if (PasswordSecureString.Length > 0)
                {
                    enteredValueHash = PasswordHashing.CalculateHash(SecureStringManipulation.ConvertSecureStringToByteArray(PasswordSecureString));
                }
                else
                {
                    ErrorMessage = "Παρακαλώ εισάγετε κωδικό.";
                    return;
                }

                if (!PasswordHashing.SequenceEquals(enteredValueHash, userFound.HashedPassword))
                {
                    ErrorMessage = "Λάθος κωδικός.";
                    return;
                }

                ErrorMessage = "Επιτυχής σύνδεση!";
                Helpers.StaticResources.User = userFound;
                MessengerInstance.Send(new LoginLogOutMessage(true));

            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public async override Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            Users = (await startingRepository.GetAllAsync<User>()).ToList();
        }

        public override Task ReloadAsync()
        {
            throw new NotImplementedException();
        }
    }
}