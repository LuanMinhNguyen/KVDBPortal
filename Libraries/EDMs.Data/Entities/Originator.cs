namespace EDMs.Data.Entities
{
    public partial class Originator
    {
        public string FullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.Description)
                        ? this.Name + " - " + this.Description
                        : this.Name;
            }
        }
    }
}
