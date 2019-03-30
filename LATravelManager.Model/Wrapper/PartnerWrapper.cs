using LATravelManager.Models;
using LATravelManager.UI.Data.LocalModels;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace LATravelManager.UI.Wrapper
{
    public class PartnerWrapper : ModelWrapper<Partner>
    {

        #region Constructors

        public PartnerWrapper() : this(new Partner())
        {
        }

        public PartnerWrapper(Partner model) : base(model)
        {
            EmailsList = !string.IsNullOrEmpty(Emails) ? new ObservableCollection<Email>(Emails.Split(',').Select(p => new Email ( p))) : new ObservableCollection<Email>();
            foreach (var item in EmailsList)
            {
                item.PropertyChanged += EmailPropertyChanged;
            }
            EmailsList.CollectionChanged += EmailsList_Changed;
        }

        #endregion Constructors

        #region Fields


        #endregion Fields

        #region Properties

        public string Emails
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        private ObservableCollection<Email> _EmailsList;

        public ObservableCollection<Email> EmailsList
        {
            get
            {
                return _EmailsList;
            }

            set
            {
                if (_EmailsList == value)
                {
                    return;
                }

                _EmailsList = value;
                RaisePropertyChanged();

            }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public string Note
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Tel
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return Name;
        }

        private void EmailPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Emails = string.Join(",", EmailsList);
            ClearErrors(nameof(EmailsList));
            if (EmailsList.Any(m => !string.IsNullOrEmpty(m.error)))
            {
                AddError(nameof(EmailsList), "Ελέγξτε τα email");
            }
        }

        private void EmailsList_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Email item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EmailPropertyChanged;
                }
                Emails = string.Join(",", EmailsList);
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (Email item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EmailPropertyChanged;
                }
                Emails = string.Join(",", EmailsList);
            }


        }

        #endregion Methods
    }
}