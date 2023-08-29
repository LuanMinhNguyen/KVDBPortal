// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocPropertiesViewDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class DocPropertiesViewDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocPropertiesViewDAO"/> class.
        /// </summary>
        public DocPropertiesViewDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DocPropertiesView> GetIQueryable()
        {
            return this.EDMsDataContext.DocPropertiesViews;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocPropertiesView> GetAll()
        {
            return this.EDMsDataContext.DocPropertiesViews.ToList();
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
        public DocPropertiesView GetById(int id)
        {
            return this.EDMsDataContext.DocPropertiesViews.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE
        ////public List<DocPropertiesView> GetAllWithoutDeparment()
        ////{
        ////    return this.EDMsDataContext.DocPropertiesViews.Where(t => t.RoleId == 0 || t.RoleId == null).ToList();
        ////}

        ////public List<DocPropertiesView> GetAllByDeparment(int deparmentId)
        ////{
        ////    return this.EDMsDataContext.DocPropertiesViews.Where(t => t.RoleId == deparmentId).ToList();
        ////} 

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
        public int? Insert(DocPropertiesView ob)
        {
            try
            {
                this.EDMsDataContext.AddToDocPropertiesViews(ob);
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
        public bool Update(DocPropertiesView src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.DocPropertiesViews
                                where rs.ID == src.ID
                                select rs).First();

                des.CategoryId = src.CategoryId;
                des.RoleId = src.RoleId;
                des.PropertyIndex = src.PropertyIndex;

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
        public bool Delete(DocPropertiesView src)
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
