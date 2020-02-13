using System;
using LATravelManager.Model.Excursions;

namespace LATravelManager.Model.Wrapper
{
    public class OptionalExcursionsWrapper : ModelWrapper<OptionalExcursion>
    {
        public OptionalExcursionsWrapper() : this(new OptionalExcursion())
        {
        }

        public OptionalExcursionsWrapper(OptionalExcursion model) : base(model)
        {
            Title = "Η προαιρετική";
        }

        public Excursion Excursion
        {
            get { return GetValue<Excursion>(); }
            set { SetValue(value); }
        }

        public DateTime Date
        {
            get { return GetValue<DateTime>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }

        public int Cost
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }
    }
}