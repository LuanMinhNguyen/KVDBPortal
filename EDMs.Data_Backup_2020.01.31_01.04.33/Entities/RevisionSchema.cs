﻿namespace EDMs.Data.Entities
{
    public partial class RevisionSchema
    {
        public string FullName
        {
            get
            {
                return !string.IsNullOrEmpty(this.Description)
                        ? this.Code + ", " + this.Description
                        : this.Code;
            }
        }
    }
}
