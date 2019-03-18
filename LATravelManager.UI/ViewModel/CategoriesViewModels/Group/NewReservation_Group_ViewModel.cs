using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using GalaSoft.MvvmLight.CommandWpf;
using LATravelManager.Models;
using LATravelManager.UI.Data.Workers;
using LATravelManager.UI.Helpers;
using LATravelManager.UI.Message;
using LATravelManager.UI.Repositories;
using LATravelManager.UI.ViewModel.BaseViewModels;
using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.ViewModel.CategoriesViewModels.Group
{
    public class NewReservation_Group_ViewModel : NewReservationGroup_Base
    {
        private bool _enabled;
        #region Constructors

        public NewReservation_Group_ViewModel()
        {
       
            MessengerInstance.Register<SelectedExcursionChangedMessage>(this, async exc => { await SelectedExcursionChanged(exc.SelectedExcursion); });
        }

        #endregion Constructors

        #region Properties

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled == value)
                {
                    return;
                }

                _enabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

       

        public override async Task LoadAsync(int id = 0, MyViewModelBase previousViewModel = null)
        {
            try
            {
                if (SelectedExcursion != null || id > 0)
                {
                    var booking = id > 0
                        ? await GenericRepository.GetFullBookingByIdAsync(id)
                        : await CreateNewBooking();

                    InitializeBooking(booking);

                    await ResetAllRefreshableDataASync();
                }
            }
            catch (Exception ex)
            {
                MessengerInstance.Send(new ShowExceptionMessage_Message(ex.Message));
            }
            finally
            {
                IsLoaded = true;
            }
        }

        public override async Task ReloadAsync()
        {
            await ResetAllRefreshableDataASync();
        }

        public async Task SelectedExcursionChanged(ExcursionWrapper selectedExcursion)
        {
            if (selectedExcursion != null)
            {
                Enabled = true;
                SelectedExcursion = selectedExcursion;
                InitializeBooking(await CreateNewBooking());

                await ResetAllRefreshableDataASync();
            }
            else
            {
                Enabled = false;
            }
        }



      
        #endregion Methods
    }
}