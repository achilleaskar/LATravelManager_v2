using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.BookingData;
using LATravelManager.Model.Extensions;
using LATravelManager.Model.People;
using LATravelManager.Model.Security;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace LATravelManager.UI.ViewModel.Management
{
    public class LoginDataManagement_ViewModel : ViewModelBase
    {
        #region Constructors

        public LoginDataManagement_ViewModel()
        {
            Context = new GenericRepository();
            SaveChangesCommand = new RelayCommand(async () => await SaveChanges(), HasChanges);
        }

        #endregion Constructors

        #region Fields

        private string _ErrorMessage;

        private ObservableCollection<LoginData> _Logins;

        private LoginData _SelectedLoginData;

        private List<string> errors = new List<string>();

        #endregion Fields

        #region Properties

        public GenericRepository Context { get; set; }

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

        public ObservableCollection<LoginData> Logins
        {
            get
            {
                return _Logins;
            }

            set
            {
                if (_Logins == value)
                {
                    return;
                }

                _Logins = value;
                Logins.CollectionChanged += Logins_CollectionChanged;
                RaisePropertyChanged();
            }
        }

        public RelayCommand SaveChangesCommand { get; }

        public LoginData SelectedLoginData
        {
            get
            {
                return _SelectedLoginData;
            }

            set
            {
                if (_SelectedLoginData == value)
                {
                    return;
                }

                _SelectedLoginData = value;
                if (SelectedLoginData != null)
                    SelectedLoginData.PropertyChanged += SelectedLoginData_PropertyChanged;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private ObservableCollection<ChangeInBooking> _Changes;

        public ObservableCollection<ChangeInBooking> Changes
        {
            get
            {
                return _Changes;
            }

            set
            {
                if (_Changes == value)
                {
                    return;
                }

                _Changes = value;
                RaisePropertyChanged();
            }
        }

        public async Task LoadAsync()
        {
            if (Context == null)
            {
                Context = new GenericRepository();
            }
            await Context.LoadAllLoginsAsync();
            await Context.LoadAllLoginDataChangesAsync();
            Logins = Context.Context.LoginData.Local;
            Changes = Context.Context.ChangesInBooking.Local;

            foreach (var item in Logins)
            {
                item.Password = AesOperation.DecryptString(StaticResources.LoginDataEncryptionKey, item.EncryptedPassword);
                item.LoadedPassword = AesOperation.DecryptString(StaticResources.LoginDataEncryptionKey, item.EncryptedPassword);
            }
        }

        internal async Task CaptureChanges()
        {
            Context.Context.ChangeTracker.DetectChanges();
            var changes = from e in Context.Context.ChangeTracker.Entries()
                          where e.State != EntityState.Unchanged
                          select e;

            if (changes.Any())
            {
                var sb = new StringBuilder();
                foreach (DbEntityEntry change in changes.Where(c => c.Entity is LoginData))
                {
                    if (change.State == EntityState.Modified && change.Entity is LoginData lde)
                    {
                        sb.Append("Αλλαγή");
                        if (change.OriginalValues[nameof(LoginData.Name)] != change.CurrentValues[nameof(LoginData.Name)])
                        {
                            sb.Append($" όνομα από: '{change.OriginalValues[nameof(LoginData.Name)]}' σε '{change.CurrentValues[nameof(LoginData.Name)]}',");
                        }
                        else
                        {
                            sb.Append($" όνομα {change.OriginalValues[nameof(LoginData.Name)]},");
                        }
                        if (change.OriginalValues[nameof(LoginData.Link)] != change.CurrentValues[nameof(LoginData.Link)])
                        {
                            sb.Append($" link από: '{change.OriginalValues[nameof(LoginData.Link)]}' σε '{change.CurrentValues[nameof(LoginData.Link)]}',");
                        }
                        else
                        {
                            sb.Append($" link {change.OriginalValues[nameof(LoginData.Link)]},");
                        }
                        if (change.OriginalValues[nameof(LoginData.Username)] != change.CurrentValues[nameof(LoginData.Username)])
                        {
                            sb.Append($" όνομα χρήστη από: '{change.OriginalValues[nameof(LoginData.Username)]}' σε '{change.CurrentValues[nameof(LoginData.Username)]}',");
                        }
                        if (lde.LoadedPassword != lde.Password)
                        {
                            sb.Append($" κωδικός από: '{lde.LoadedPassword}' σε '{lde.Password}',");
                        }

                        if (sb.Length > 7)
                            Context.Add(new ChangeInBooking { Date = DateTime.Now, Description = sb.ToString().TrimEnd(','), User = await Context.GetByIdAsync<User>(StaticResources.User.Id), ChangeType = Model.ChangeType.LoginData });
                    }
                    else if (change.State == EntityState.Added && change.Entity is LoginData ld)
                    {
                        Context.Add(new ChangeInBooking { Date = DateTime.Now, Description = $"Προστέθηκαν στοιχεία σύνδεσης: όνομα: {ld.Name}, link: {ld.Link}, όνομα χρήστη: {ld.Username}, κωδικός {ld.Password}", User = await Context.GetByIdAsync<User>(StaticResources.User.Id), ChangeType = Model.ChangeType.LoginData });
                    }
                    else if (change.State == EntityState.Deleted && change.Entity is LoginData ldd)
                    {
                        Context.Add(new ChangeInBooking { Date = DateTime.Now, Description = $"Διαγράφηκαν στοιχεία σύνδεσης: όνομα: {ldd.Name}, link: {ldd.Link}, όνομα χρήστη: {ldd.Username}, κωδικός {ldd.Password}", User = await Context.GetByIdAsync<User>(StaticResources.User.Id), ChangeType = Model.ChangeType.LoginData });
                    }
                }
            }
        }

        private bool CanSaveChanges()
        {
            foreach (var logindata in Logins)
            {
                if (!logindata.IsValid(ref errors))
                {
                    return false;
                }
            }
            return true;
        }

        private bool HasChanges()
        {
            return Context.HasChanges();
        }

        private void Logins_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (LoginData item in e.NewItems)
                {
                    item.LastEditedBy = Context.GetById<User>(StaticResources.User.Id);
                    item.ModifiedDate = DateTime.Now;
                }
            }
        }

        private async Task SaveChanges()
        {
            if (CanSaveChanges())
            {
                Mouse.OverrideCursor = Cursors.Wait;
                foreach (var item in Logins)
                {
                    if (item.Id == 0)
                    {
                        item.ModifiedDate = DateTime.Now;
                    }
                }
                CaptureChanges();
                await Context.SaveAsync();
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            else
            {
                ErrorMessage = "ΔΕΝ έγινε αποθήκευση, υπάρχουν σφάλματα";
            }
        }

        private void SelectedLoginData_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (SelectedLoginData != null && e.PropertyName == nameof(LoginData.Password))
            {
                SelectedLoginData.EncryptedPassword = AesOperation.EncryptString(StaticResources.LoginDataEncryptionKey, SelectedLoginData.Password);
            }
        }

        #endregion Methods
    }
}