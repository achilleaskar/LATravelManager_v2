using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model.LocalModels;
using LATravelManager.Model.People;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LATravelManager.UI.ViewModel.Management
{
    public class PartnerManagement_ViewModel : AddEditBase<PartnerWrapper, Partner>
    {
        public PartnerManagement_ViewModel(BasicDataManager context) : base(context)
        {
            ControlName = "Διαχείριση Συνεργατών";
            AddEmailCommand = new RelayCommand(AddEmail, CanAddEmail);
        }

        public async Task GetAllCompaniesAsync(bool allProperties = false)
        {
            MessengerInstance.Send(new IsBusyChangedMessage(true));
            Companies = new ObservableCollection<Company>((await BasicDataManager.Context.GetAllCompaniesAsync(false)).OrderBy(r => r.CompanyName));
            MessengerInstance.Send(new IsBusyChangedMessage(false));
        }

        private ObservableCollection<Company> _Companies;

        public ObservableCollection<Company> Companies
        {
            get
            {
                return _Companies;
            }

            set
            {
                if (_Companies == value)
                {
                    return;
                }

                _Companies = value;
                RaisePropertyChanged();
            }
        }

        private bool CanAddEmail()
        {
            try
            {
                if (SelectedEntity == null || string.IsNullOrEmpty(SelectedEntity.NewEmail))
                    return false;

                return new EmailAddressAttribute().IsValid(SelectedEntity.NewEmail);
            }
            catch
            {
                return false;
            }
        }

        private void AddEmail()
        {
            SelectedEntity.EmailsList.Add(new Email(SelectedEntity.NewEmail));
        }

        public override void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null)
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));

                MainCollection = new ObservableCollection<PartnerWrapper>((BasicDataManager.Partners).Select(p => new PartnerWrapper(p)));

            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                MessengerInstance.Send(new IsBusyChangedMessage(false));
            }
        }

        public RelayCommand AddEmailCommand { get; set; }
    }
}