// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentNew.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the DocumentNew type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.Entities
{
    using System.Linq;

    using EDMs.Data.DAO.Document;

    /// <summary>
    /// The document new.
    /// </summary>
    public partial class DocumentNew
    {
        /// <summary>
        /// Gets the revision full name.
        /// </summary>
        public string RevisionFullName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.RevName))
                {
                    return this.RevName + "_" + this.Name;
                }

                return this.Name;
            }
        }

        /// <summary>
        /// Gets the default doc.
        /// </summary>
        public AttachFile DefaultDoc
        {
            get
            {
                var attachFileDAO = new AttachFileDAO();
                return attachFileDAO.GetAll().FirstOrDefault(t => t.DocumentId == this.ID && t.IsDefault == true);
            }
        }

        public int AttachFileCount
        {
            get
            {
                var attachFileDAO = new AttachFileDAO();
                return attachFileDAO.GetAll().Count(t => t.DocumentId == this.ID);
            }
        }
    }
}
