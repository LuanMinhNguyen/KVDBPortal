// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemCodeDAO.cs" company="">
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

    using Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class SystemCodeDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SystemCodeDocDAO"/> class.
        /// </summary>
        public SystemCodeDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<SystemCode> GetIQueryable()
        {
            return this.EDMsDataContext.SystemCodes;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<SystemCode> GetAll()
        {
            return this.EDMsDataContext.SystemCodes.ToList();
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
        public SystemCode GetById(int id)
        {
            return this.EDMsDataContext.SystemCodes.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(SystemCode ob)
        {
            try
            {
                this.EDMsDataContext.AddToSystemCodes(ob);
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
        public bool Update(SystemCode src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.SystemCodes
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.Active = src.Active;
                des.ParentCode = src.ParentCode;
                des.ParentId = src.ParentId;
                des.ParentName = src.ParentName;
                des.PackageNo = src.PackageNo;
                des.KKSCode = src.KKSCode;
                des.KKSFullName = src.KKSFullName;
                des.KKSId = src.KKSId;

                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

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
        public bool Delete(SystemCode src)
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
