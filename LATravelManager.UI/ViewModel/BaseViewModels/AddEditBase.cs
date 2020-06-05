using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Model;
using LATravelManager.Model.Wrapper;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.ViewModel.BaseViewModels;

namespace LaTravelManager.BaseTypes
{
    public abstract class AddEditBase<TWrapper, TEntity> : ViewModelBase
        where TEntity : BaseModel, new()
        where TWrapper : ModelWrapper<TEntity>, new()
    {
        #region Constructors

        protected AddEditBase(BasicDataManager basicDataManager)
        {
            AddEntityCommand = new RelayCommand(AddEntity, CanAddEntity);
            UpdateEntitiesCommand = new RelayCommand(async () => { await ReloadEntities(); }, true);
            SaveChangesCommand = new RelayCommand(SaveChanges, CanSaveChanges);
            ClearEntityCommand = new RelayCommand(ClearEntity, CanClearEntity);
            RemoveEntityCommand = new RelayCommand(RemoveEntity, CanRemoveEntity);

            MainCollection = new ObservableCollection<TWrapper>();

            BasicDataManager = basicDataManager;

            SelectedEntity = new TWrapper();
        }

        public abstract void ReLoad(int id = 0, MyViewModelBaseAsync previousViewModel = null);

        private void MainCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (TWrapper item in e.OldItems)
                {
                    //Removed items
                    item.PropertyChanged -= EntityViewModelPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (TWrapper item in e.NewItems)
                {
                    //Added items
                    item.PropertyChanged += EntityViewModelPropertyChanged;
                }
            }
        }

        #endregion Constructors

        #region Fields

        private ObservableCollection<TWrapper> _MainCollection;

        private string _ResultMessage;

        private TWrapper _SelectedEntity;

        #endregion Fields

        private string _ControlName;

        public string ControlName
        {
            get
            {
                return _ControlName;
            }

            set
            {
                if (_ControlName == value)
                {
                    return;
                }

                _ControlName = value;
                RaisePropertyChanged();
            }
        }

        #region Properties

        /// <summary>
        /// Adds current entity to database and saves
        /// </summary>
        public RelayCommand AddEntityCommand { get; }

        /// <summary>
        /// Creates a new entity clearing all custom values
        /// </summary>
        public RelayCommand ClearEntityCommand { get; }

        public BasicDataManager BasicDataManager { get; set; }

        /// <summary>
        /// Sets and gets the MainCollection property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public ObservableCollection<TWrapper> MainCollection
        {
            get
            {
                return _MainCollection;
            }

            set
            {
                if (_MainCollection == value)
                {
                    return;
                }

                _MainCollection = value;
                MainCollection.CollectionChanged += MainCollectionChanged;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Removes selected entity from Database
        /// </summary>
        public RelayCommand RemoveEntityCommand { get; }

        public string ResultMessage
        {
            get
            {
                return _ResultMessage;
            }

            set
            {
                if (_ResultMessage == value)
                {
                    return;
                }

                _ResultMessage = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Saves changes done to all entities since last udpate
        /// </summary>
        public RelayCommand SaveChangesCommand { get; }

        /// <summary>
        /// Sets and gets the SelectedEntity property. Changes to that property's value raise the
        /// PropertyChanged event.
        /// </summary>
        public TWrapper SelectedEntity
        {
            get
            {
                return _SelectedEntity;
            }

            set
            {
                if (_SelectedEntity == value)
                {
                    return;
                }
                if (value == null)
                {
                    _SelectedEntity = new TWrapper();
                }

                _SelectedEntity = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///Updates the list of entities with latset Db values
        /// </summary>
        public RelayCommand UpdateEntitiesCommand { get; }

        #endregion Properties

        #region Methods

        public virtual bool CanSaveChanges()
        {
            return BasicDataManager.HasChanges() && SelectedEntity != null && !SelectedEntity.HasErrors;
        }

        public void EntityViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ResultMessage = "";
        }

        public async virtual void SaveChanges()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                await BasicDataManager.SaveAsync();
                ResultMessage = "Όι αλλαγές απόθηκεύτηκαν επιτυχώς";
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

        private void AddEntity()
        {
            try
            {
                BasicDataManager.Add(SelectedEntity.Model);
                MainCollection.Add(SelectedEntity);
                // ClearEntity();
                ResultMessage = SelectedEntity.Title + " προστέθηκε επιτυχώς";
                RemoveEntityCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        public virtual bool CanAddEntity()
        {
            return SelectedEntity != null && !SelectedEntity.HasErrors && SelectedEntity.Id == 0;
        }

        private bool CanClearEntity()
        {
            return SelectedEntity != null;//&& SelectedEntity.HasValues();
        }

        private bool CanRemoveEntity()
        {
            return SelectedEntity != null && SelectedEntity.Id > 0;
        }

        private void ClearEntity()
        {
            SelectedEntity = new TWrapper();
            ResultMessage = "";
        }

        protected async Task ReloadEntities()
        {
            try
            {
                MessengerInstance.Send(new IsBusyChangedMessage(true));
                ResultMessage = "";
                //if (BasicDataManager != null && !BasicDataManager.IsTaskOk)
                //{
                //    await BasicDataManager.LastTask;
                //}

                await BasicDataManager.Refresh();
                await BasicDataManager.LoadAsync();
                if (SelectedEntity.Id > 0)
                {
                    SelectedEntity = new TWrapper();
                }
                ReLoad();
                ResultMessage = "Η ενημέρωση ολοκληρώθηκε!";
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

        private void RemoveEntity()
        {
            try
            {
                BasicDataManager.Delete(SelectedEntity.Model);
                MainCollection.Remove(SelectedEntity);
                SelectedEntity = new TWrapper();
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
        }

        #endregion Methods
    }
}