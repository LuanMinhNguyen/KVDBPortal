// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentCodeDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Library
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class DocumentCodeDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDocumentCodeDAO"/> class.
        /// </summary>
        public DocumentCodeDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DocumentCode> GetIQueryable()
        {
            return this.EDMsDataContext.DocumentCodes;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentCode> GetAll()
        {
            return this.EDMsDataContext.DocumentCodes.OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public DocumentCode GetById(int id)
        {
            return this.EDMsDataContext.DocumentCodes.FirstOrDefault(ob => ob.ID == id);
        }
        public DocumentCode GetByCode(string code)
        {
            return this.EDMsDataContext.DocumentCodes.FirstOrDefault(ob => ob.Code == code);
        }
        #endregion

        #region GET ADVANCE

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(DocumentCode ob)
        {
            try
            {
                this.EDMsDataContext.AddToDocumentCodes(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="src">
        /// Entity for update
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if update success, false if not
        /// </returns>
        public bool Update(DocumentCode src)
        {
            try
            {
                DocumentCode des = (from rs in this.EDMsDataContext.DocumentCodes
                                where rs.ID == src.ID
                                select rs).First();

                des.Code = src.Code;
                des.Name = src.Name;
                des.Description = src.Description;

                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;
                des.Active = src.Active;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if delete success, false if not
        /// </returns>
        public bool Delete(DocumentCode src)
        {
            try
            {
                var des = this.GetById(src.ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="ID"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                var des = this.GetById(ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentCode"/>.
        /// </returns>
        public DocumentCode GetByName(string name)
        {
            return this.EDMsDataContext.DocumentCodes.FirstOrDefault(ob => ob.Name == name);
        }
    }
}
