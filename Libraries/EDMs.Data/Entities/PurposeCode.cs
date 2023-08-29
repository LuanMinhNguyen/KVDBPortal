namespace EDMs.Data.Entities
{
    public partial class PurposeCode
    {
        public string FullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.Description)
                        ? this.Code + " - " + this.Description
                        : this.Code;
            }
        }
    }
}
