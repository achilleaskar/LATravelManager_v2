﻿using LATravelManager.Model.BookingData;
using LATravelManager.Model.Locations;
using LATravelManager.Model.People;
using LATravelManager.UI.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace LATravelManager.UI.Helpers
{
    public static class StaticResources
    {
        #region Properties

        public static string[] AgesList { get; set; } = { "<1", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18+" };

        public static string[] Airports { get; set; } = { "Thessaloniki(SKG)", "Athens(ATH)" };
        public static ObservableCollection<StartingPlace> StartingPlaces { get; set; }
        public static Booking TmpBooking { get; set; }
        public static User User { get; set; }

        #endregion Properties

        public static async void CalculateSum(GenericRepository genericRepository)
        {
           IEnumerable<Booking> bookings = await genericRepository.GetAllBookingInPeriodNoTracking(new DateTime(2018, 10, 1), new DateTime(2019, 04, 1), 2);
            decimal sum = 0;
            foreach (Booking b in bookings)
            {
                if (b.IsPartners)
                {
                    sum += b.NetPrice;
                }
                else
                {
                    foreach (Reservation r in b.ReservationsInBooking)
                    {
                        foreach (Customer c in r.CustomersList)
                        {
                            sum += c.Price;
                        }
                    }
                }
            }
        }

        ///<summary>Finds the index of the first item matching an expression in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="predicate">The expression to test the items against.</param>
        ///<returns>The index of the first matching item, or -1 if no items match.</returns>
        public static int FindIndex<T>(this IEnumerable<T> items, Func<T, bool> predicate)
        {
            if (items == null) throw new ArgumentNullException("items");
            if (predicate == null) throw new ArgumentNullException("predicate");

            int retVal = 0;
            foreach (var item in items)
            {
                if (predicate(item)) return retVal;
                retVal++;
            }
            return -1;
        }
        ///<summary>Finds the index of the first occurrence of an item in an enumerable.</summary>
        ///<param name="items">The enumerable to search.</param>
        ///<param name="item">The item to find.</param>
        ///<returns>The index of the first matching item, or -1 if the item was not found.</returns>
        public static int IndexOf2<T>(this IEnumerable<T> items, T item) { return items.FindIndex(i => EqualityComparer<T>.Default.Equals(item, i)); }
    }

    public class RequiredIfAttribute : ValidationAttribute
    {
        #region Constructors

        public RequiredIfAttribute(string dependentProperty, object targetValue)
        {
            _dependentProperty = dependentProperty;
            _targetValue = targetValue;
        }

        #endregion Constructors

        #region Fields

        private readonly RequiredAttribute _innerAttribute = new RequiredAttribute();

        #endregion Fields

        #region Properties

        public string _dependentProperty { get; set; }
        public object _targetValue { get; set; }

        #endregion Properties

        #region Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            System.Reflection.PropertyInfo field = validationContext.ObjectType.GetProperty(_dependentProperty);
            if (field != null)
            {
                object dependentValue = field.GetValue(validationContext.ObjectInstance, null);
                if ((dependentValue == null && _targetValue == null) || (dependentValue.Equals(_targetValue)))
                {
                    if (!_innerAttribute.IsValid(value))
                    {
                        string name = validationContext.DisplayName;
                        return new ValidationResult(ErrorMessage = name + " Is required.");
                    }
                }
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(FormatErrorMessage(_dependentProperty));
            }
        }

        #endregion Methods
    }
}