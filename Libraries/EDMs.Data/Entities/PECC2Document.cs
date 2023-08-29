namespace EDMs.Data.Entities
{
    public partial class PECC2Documents
    {
        public string DocNoWithRev
        {
            get
            {
                return !string.IsNullOrEmpty(this.Revision)
                        ? this.DocNo + " (Rev: " + this.Revision + ")"
                        : this.DocNo;
            }
        }

        public bool IsChangeRequest { get; set; }
    }
}
