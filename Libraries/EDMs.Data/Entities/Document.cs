// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Document.cs" company="">
//   
// </copyright>
// <summary>
//   Defines the Document type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Document;
    using EDMs.Data.DAO.Library;
    using EDMs.Data.DAO.Security;

    /// <summary>
    /// The document.
    /// </summary>
    public partial class Document
    {
        /// <summary>
        /// Gets the folder doc.
        /// </summary>
        public Folder FolderDoc
        {
            get
            {
                var folderDao = new FolderDAO();
                return folderDao.GetById(this.FolderID.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the created user.
        /// </summary>
        public User CreatedUser
        {
            get
            {
                var userDAO = new UserDAO();
                return userDAO.GetByID(this.CreatedBy.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the last updated user.
        /// </summary>
        public User LastUpdatedUser
        {
            get
            {
                var userDAO = new UserDAO();
                return userDAO.GetByID(this.LastUpdatedBy.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the patient status.
        /// </summary>
        public Revision Revision
        {
            get
            {
                var revisionDAO = new RevisionDAO();
                return revisionDAO.GetById(this.RevisionID.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        public Status Status
        {
            get
            {
                var statusDAO = new StatusDAO();
                return statusDAO.GetById(this.StatusID.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the discipline.
        /// </summary>
        public Discipline Discipline
        {
            get
            {
                var disciplineDAO = new DisciplineDAO();
                return disciplineDAO.GetById(this.DisciplineID.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the document type.
        /// </summary>
        public DocumentType DocumentType
        {
            get
            {
                var documentTypeDAO = new DocumentTypeDAO();
                return documentTypeDAO.GetById(this.DocumentTypeID.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets the received from.
        /// </summary>
        public ReceivedFrom ReceivedFrom
        {
            get
            {
                var receivedFromDAO = new ReceivedFromDAO();
                return receivedFromDAO.GetById(this.ReceivedFromID.GetValueOrDefault());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether is folder.
        /// </summary>
        public bool IsFolder { get; set; }
    }
}
