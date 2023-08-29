// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AA_MaterialRequestDAO.cs" company="">
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
    public class AA_MaterialRequestDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AA_MaterialRequestDAO"/> class.
        /// </summary>
        public AA_MaterialRequestDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<AA_MaterialRequest> GetIQueryable()
        {
            return this.EDMsDataContext.AA_MaterialRequest;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AA_MaterialRequest> GetAll()
        {
            return this.EDMsDataContext.AA_MaterialRequest.OrderByDescending(t => t.ID).ToList();
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
        public AA_MaterialRequest GetById(Guid id)
        {
            return this.EDMsDataContext.AA_MaterialRequest.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(AA_MaterialRequest ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_MaterialRequest(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch (Exception ex)
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
        public bool Update(AA_MaterialRequest src)
        {
            try
            {
                AA_MaterialRequest des = (from rs in this.EDMsDataContext.AA_MaterialRequest
                                where rs.ID == src.ID
                                select rs).First();

                des.Code = src.Code;
                des.Description = src.Description;
                des.OrganizationCode = src.OrganizationCode;
                des.OrganizationName = src.OrganizationName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedById = src.UpdatedById;
                des.UpdatedDate = src.UpdatedDate;
                des.StoreCode = src.StoreCode;
                des.StoreName = src.StoreName;
                des.RequestDate = src.RequestDate;
                des.RequestBy = src.RequestBy;
                des.Note = src.Note;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;

                des.SoLayMauXN = src.SoLayMauXN;
                des.SoNguoiCachLyTaiNha = src.SoNguoiCachLyTaiNha;
                des.SoNguoiCachLyTapTrung = src.SoNguoiCachLyTapTrung;
                des.SoTYT = src.SoTYT;
                des.IsGenEAMMR = src.IsGenEAMMR;
                des.IsMonthlyRequest = src.IsMonthlyRequest;
                des.RequestForMonth = src.RequestForMonth;

                des.DocumentaryNo = src.DocumentaryNo;
                des.DocumentaryDate = src.DocumentaryDate;
                des.DocumentaryReason = src.DocumentaryReason;
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
        public bool Delete(AA_MaterialRequest src)
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
