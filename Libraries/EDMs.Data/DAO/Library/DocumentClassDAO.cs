// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentClassDAO.cs" company="">
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
    public class DocumentClassDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentClassDAO"/> class.
        /// </summary>
        public DocumentClassDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DocumentClass> GetIQueryable()
        {
            return this.EDMsDataContext.DocumentClasses;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentClass> GetAll()
        {
            return this.EDMsDataContext.DocumentClasses.ToList();
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
        public DocumentClass GetById(int id)
        {
            return this.EDMsDataContext.DocumentClasses.FirstOrDefault(ob => ob.ID == id);
        }
        public DocumentClass GetByName(string _Name)
        {
            return this.EDMsDataContext.DocumentClasses.FirstOrDefault(ob => ob.Name == _Name);
        }

        public DocumentClass GetByCode(string _Code)
        {
            return this.EDMsDataContext.DocumentClasses.FirstOrDefault(ob => ob.Code == _Code);
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
        public int? Insert(DocumentClass ob)
        {
            try
            {
                this.EDMsDataContext.AddToDocumentClasses(ob);
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
        public bool Update(DocumentClass src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.DocumentClasses
                           where rs.ID == src.ID
                           select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.Code = src.Code;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;

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
        public bool Delete(DocumentClass src)
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
    }
}
