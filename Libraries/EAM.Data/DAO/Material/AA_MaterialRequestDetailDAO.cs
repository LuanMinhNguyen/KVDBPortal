// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AA_MaterialRequestDetailDAO.cs" company="">
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
    public class AA_MaterialRequestDetailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AA_MaterialRequestDetailDAO"/> class.
        /// </summary>
        public AA_MaterialRequestDetailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<AA_MaterialRequestDetail> GetIQueryable()
        {
            return this.EDMsDataContext.AA_MaterialRequestDetail;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AA_MaterialRequestDetail> GetAll()
        {
            return this.EDMsDataContext.AA_MaterialRequestDetail.OrderByDescending(t => t.ID).ToList();
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
        public AA_MaterialRequestDetail GetById(Guid id)
        {
            return this.EDMsDataContext.AA_MaterialRequestDetail.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(AA_MaterialRequestDetail ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_MaterialRequestDetail(ob);
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
        public bool Update(AA_MaterialRequestDetail src)
        {
            try
            {
                AA_MaterialRequestDetail des = (from rs in this.EDMsDataContext.AA_MaterialRequestDetail
                                where rs.ID == src.ID
                                select rs).First();

                des.MaterialRequestID = src.MaterialRequestID;
                des.PartCode = src.PartCode;
                des.PartDescription = src.PartDescription;
                des.PartUMO = src.PartUMO;
                des.CurrentStock = src.CurrentStock;
                des.RequestQty = src.RequestQty;
                des.ApprovedQty = src.ApprovedQty;
                des.FromStoreCode = src.FromStoreCode;
                des.FromStoreName = src.FromStoreName;
                des.ToStoreCode = src.ToStoreCode;
                des.ToStoreName = src.ToStoreName;
                des.RequestPartName = src.RequestPartName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedById = src.UpdatedById;
                des.UpdatedDate = src.UpdatedDate;
                des.EAMPartCode = src.EAMPartCode;
                des.EAMPartDescription = src.EAMPartDescription;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch(Exception ex)
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
        public bool Delete(AA_MaterialRequestDetail src)
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
