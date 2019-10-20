using LATravelManager.Model.Lists;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LATravelManager.Model.Wrapper
{
    public class LeaderWrapper : ModelWrapper<Leader>
    {
        public LeaderWrapper() : this(new Leader())
        {

        }

        public LeaderWrapper(Leader model) : base(model)
        {
            Title = "Ο Αρχηγός";
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value.ToUpper()); }
        }
        public string Tel
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
