namespace LATravelManager.Model
{
    public class LookupItem
    {
        public int Id { get; set; }

        public string DisplayMember { get; set; }
    }

    public class NullLookupItem : LookupItem
    {
        public static new int? Id { get { return null; } }
    }
}