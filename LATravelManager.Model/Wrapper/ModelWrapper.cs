using LATravelManager.UI.Wrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LATravelManager.Model.Wrapper
{
    public class ModelWrapper<T> : NotifyDataErrorInfoBase where T : BaseModel
    {
        public ModelWrapper(T model)
        {
            try
            {
                Model = model;
                if (model.Id == 0)
                {
                    ValidateAllProperties();
                }
            }
            catch (Exception ex)
            {

                
            }
        }

        public bool HasValues()
        {
            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(this);
                    if (!string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
                else if (pi.PropertyType == typeof(int))
                {
                    int value = (int)pi.GetValue(this);
                    if (value != 0)
                    {
                        return true;
                    }
                }
                else if (pi.PropertyType == typeof(double))
                {
                    int value = (int)pi.GetValue(this);
                    if (value != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
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

        public void ValidateAllProperties()
        {
            foreach (PropertyInfo pi in Model.GetType().GetProperties())
            {
                ValidatePropertyInternal(pi.Name, pi.GetValue(Model));
            }
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

        protected void ValidatePropertyInternal(string propertyName, object currentValue)
        {
            ClearErrors(propertyName);

            ValidateDataAnnotations(propertyName, currentValue);

            ValidateCustomErrors(propertyName);
        }

        public string Title { get; set; }

        private void ValidateDataAnnotations(string propertyName, object currentValue)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(Model) { MemberName = propertyName };
            Validator.TryValidateProperty(currentValue, context, results);

            foreach (ValidationResult result in results)
            {
                AddError(propertyName, result.ErrorMessage);
            }
        }

        private void ValidateCustomErrors(string propertyName)
        {
            IEnumerable<string> errors = ValidateProperty(propertyName);
            if (errors != null)
            {
                foreach (string error in errors)
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