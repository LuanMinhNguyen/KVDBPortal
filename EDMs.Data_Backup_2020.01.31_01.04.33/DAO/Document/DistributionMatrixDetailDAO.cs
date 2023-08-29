// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistributionMatrixDetailDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Security.Cryptography;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class DistributionMatrixDetailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionMatrixDetailDAO"/> class.
        /// </summary>
        public DistributionMatrixDetailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DistributionMatrixDetail> GetIQueryable()
        {
            return this.EDMsDataContext.DistributionMatrixDetails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DistributionMatrixDetail> GetAll()
        {
            return this.EDMsDataContext.DistributionMatrixDetails.ToList();
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
        public DistributionMatrixDetail GetById(int id)
        {
            return this.EDMsDataContext.DistributionMatrixDetails.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get specific.
        /// </summary>
        /// <param name="tranId">
        /// The tran id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DistributionMatrixDetail> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.DistributionMatrixDetails.Where(t => t.ID == tranId).ToList();
        }
        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>int?</cref>
        ///     </see> .
        /// </returns>
        public int? Insert(DistributionMatrixDetail ob)
        {
            try
            {
                this.EDMsDataContext.AddToDistributionMatrixDetails(ob);
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
        public bool Update(DistributionMatrixDetail src)
        {
            try
            {
                DistributionMatrixDetail des = (from rs in this.EDMsDataContext.DistributionMatrixDetails
                                where rs.ID == src.ID
                                select rs).First();

                des.DistributionMatrixId = src.DistributionMatrixId;
                des.DistributionMatrixName = src.DistributionMatrixName;
                des.DisciplineId = src.DisciplineId;
                des.DisciplineName = src.DisciplineName;
                des.DisciplineFullName = src.DisciplineFullName;
                des.DocTypeId = src.DocTypeId;
                des.DocTypeName = src.DocTypeName;
                des.DocTypeFullName = src.DocTypeFullName;
                des.ActionTypeId = src.ActionTypeId;
                des.ActionTypeFullName = src.ActionTypeFullName;
                des.ActionTypeName = src.ActionTypeName;
                des.UserId = src.UserId;
                des.UserName = src.UserName;
                des.UnitCodeId = src.UnitCodeId;
                des.UnitCodeName = src.UnitCodeName;
                des.SerialNo = src.SerialNo;
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
        public bool Delete(DistributionMatrixDetail src)
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
