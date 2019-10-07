﻿using GalaSoft.MvvmLight.Messaging;
using LATravelManager.Model.Hotels;
using LATravelManager.Model.Locations;
using System;

namespace LATravelManager.Model.Services
{
    public class HotelService : Service
    {
        #region Constructors

        public HotelService()
        {
            Tittle = "Διαμονή";
            Option = DateTime.Today.AddMinutes(6);
            //PropertyChanged += HotelService_PropertyChanged;
        }

        #endregion Constructors

        #region Fields

        private City _City;

        private Hotel _Hotel;

        private DateTime _Option;

        private RoomType _RoomType;

        #endregion Fields

        #region Properties

        public bool HasOption => Option.Minute != 6;
        public City City
        {
            get
            {
                return _City;
            }

            set
            {
                if (_City == value)
                {
                    return;
                }

                _City = value;
                RaisePropertyChanged();
            }
        }

        public Hotel Hotel
        {
            get
            {
                return _Hotel;
            }

            set
            {
                if (_Hotel == value)
                {
                    return;
                }

                _Hotel = value;
                RaisePropertyChanged();
            }
        }

        public DateTime Option
        {
            get
            {
                return _Option;
            }

            set
            {
                if (_Option == value)
                {
                    return;
                }

                _Option = value.AddMinutes(-value.Minute);
                RaisePropertyChanged();
            }
        }

        public RoomType RoomType
        {
            get
            {
                return _RoomType;
            }

            set
            {
                if (_RoomType == value)
                {
                    return;
                }

                _RoomType = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties

        #region Methods

        private void HotelService_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        #endregion Methods
    }
}