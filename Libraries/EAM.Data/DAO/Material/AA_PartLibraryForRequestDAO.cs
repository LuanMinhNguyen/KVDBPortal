// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AA_PartLibraryForRequestDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.Entities;

namespace EAM.Data.DAO.Material
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class AA_PartLibraryForRequestDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AA_PartLibraryForRequestDAO"/> class.
        /// </summary>
        public AA_PartLibraryForRequestDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<AA_PartLibraryForRequest> GetIQueryable()
        {
            return this.EDMsDataContext.AA_PartLibraryForRequest;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AA_PartLibraryForRequest> GetAll()
        {
            return this.EDMsDataContext.AA_PartLibraryForRequest.OrderByDescending(t => t.ID).ToList();
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
        public AA_PartLibraryForRequest GetById(Guid id)
        {
            return this.EDMsDataContext.AA_PartLibraryForRequest.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(AA_PartLibraryForRequest ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_PartLibraryForRequest(ob);
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
        public bool Update(AA_PartLibraryForRequest src)
        {
            try
            {
                AA_PartLibraryForRequest des = (from rs in this.EDMsDataContext.AA_PartLibraryForRequest
                                where rs.ID == src.ID
                                select rs).First();

                des.PartName = src.PartName;
                des.UMO = src.UMO;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedById = src.UpdatedById;
                des.UpdatedDate = src.UpdatedDate;
                des.OrderNumber = src.OrderNumber;
                des.Formula = src.Formula;
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
        public bool Delete(AA_PartLibraryForRequest src)
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
        public bool Delete(Guid ID)
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
