﻿using LATravelManager.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace LATravelManager.UI.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase where T : BaseModel
    {
        public ModelWrapper(T model)
        {
            Model = model;
        }

        public DateTime? ModifiedDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public T Model { get; }

        public int Id
        {
            get { return Model.Id; }
            set { SetValue(value); }
        }
        protected virtual void SetValue<TValue>(TValue value,
          [CallerMemberName]string propertyName = null)
        {
            typeof(T).GetProperty(propertyName).SetValue(Model, value);
            RaisePropertyChanged(propertyName);
            ValidatePropertyInternal(propertyName, value);
        }

        protected virtual TValue GetValue<TValue>([CallerMemberName]string propertyName = null)
        {
            return (TValue)typeof(T).GetProperty(propertyName).GetValue(Model);
        }

        public DateTime CreatedDate
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        private void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            ValidateDataAnnotations(propertyName, currentValue);

            ValidateCustomErrors(propertyName);
        }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (var result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            var errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    AddError(propertyName, error);
                }
            }
        }

        protected virtual IEnumerable<string> ValidateProperty(string propertyName)
        {
            return null;
        }
    }
}