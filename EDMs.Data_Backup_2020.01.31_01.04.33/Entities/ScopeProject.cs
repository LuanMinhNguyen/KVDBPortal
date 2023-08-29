namespace EDMs.Data.Entities
{
    public partial class ScopeProject
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
