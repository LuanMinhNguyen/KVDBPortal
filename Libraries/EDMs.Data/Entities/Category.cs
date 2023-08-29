namespace EDMs.Data.Entities
{
    /// <summary>
    /// The role.
    /// </summary>
    public partial class Category
    {
        private int parentId;

        public int ParentId
        {
            get
            {
                return this.parentId;
            }
            set
            {
                this.parentId = value;
            }
        }
    }
}
