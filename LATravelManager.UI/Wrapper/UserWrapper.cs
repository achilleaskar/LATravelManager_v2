using LATravelManager.Models;
using static LATravelManager.Model.Enums;

namespace LATravelManager.UI.Wrapper
{
    public class UserWrapper : ModelWrapper<User>
    {
        #region Constructors

        public UserWrapper():base(new User())
        {
            Title = "Ο χρήστης";

        }
        public UserWrapper(User model) : base(model)
        {
            Title = "Ο χρήστης";
        }

        #endregion Constructors

        #region Properties

        public int BaseLocation
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public byte[] HashedPassword
        {
            get { return GetValue<byte[]>(); }
            set { SetValue(value); }
        }

        public int Level
        {
            get { return GetValue<int>(); }
            set { SetValue(value); }
        }

        public string Name
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Surename
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string Tel
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        public string UserName
        {
            get { return GetValue<string>(); }
            set { SetValue(value); }
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return UserName;
        }

        #endregion Methods
    }
}