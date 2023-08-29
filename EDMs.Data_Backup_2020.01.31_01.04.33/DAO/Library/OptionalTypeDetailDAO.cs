// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionalTypeDetailDAO.cs" company="">
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
    public class OptionalTypeDetailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalTypeDetailDAO"/> class.
        /// </summary>
        public OptionalTypeDetailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<OptionalTypeDetail> GetIQueryable()
        {
            return this.EDMsDataContext.OptionalTypeDetails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<OptionalTypeDetail> GetAll()
        {
            return this.EDMsDataContext.OptionalTypeDetails.ToList();
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
        public OptionalTypeDetail GetById(int id)
        {
            return this.EDMsDataContext.OptionalTypeDetails.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE
        ////public List<OptionalTypeDetail> GetAllWithoutDeparment()
        ////{
        ////    return this.EDMsDataContext.OptionalTypeDetails.Where(t => t.RoleId == 0 || t.RoleId == null).ToList();
        ////}

        ////public List<OptionalTypeDetail> GetAllByDeparment(int deparmentId)
        ////{
        ////    return this.EDMsDataContext.OptionalTypeDetails.Where(t => t.RoleId == deparmentId).ToList();
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
        public int? Insert(OptionalTypeDetail ob)
        {
            try
            {
                this.EDMsDataContext.AddToOptionalTypeDetails(ob);
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
        public bool Update(OptionalTypeDetail src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.OptionalTypeDetails
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.ParentId = src.ParentId;
                des.RoleId = src.RoleId;
                des.OptionalTypeId = src.OptionalTypeId;
                des.IsRoot = src.IsRoot;
                des.Active = src.Active;

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
        public bool Delete(OptionalTypeDetail src)
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
