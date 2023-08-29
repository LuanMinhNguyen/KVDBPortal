// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcurementRequirementDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.CostContract
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class ProcurementRequirementDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcurementRequirementDAO"/> class.
        /// </summary>
        public ProcurementRequirementDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ProcurementRequirement> GetIQueryable()
        {
            return this.EDMsDataContext.ProcurementRequirements;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProcurementRequirement> GetAll()
        {
            return this.EDMsDataContext.ProcurementRequirements.OrderByDescending(t => t.ID).ToList();
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
        public ProcurementRequirement GetById(int id)
        {
            return this.EDMsDataContext.ProcurementRequirements.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(ProcurementRequirement ob)
        {
            try
            {
                this.EDMsDataContext.AddToProcurementRequirements(ob);
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
        public bool Update(ProcurementRequirement src)
        {
            try
            {
                ProcurementRequirement des = (from rs in this.EDMsDataContext.ProcurementRequirements
                                where rs.ID == src.ID
                                select rs).First();

                des.Number = src.Number;
                des.Description = src.Description;
                des.Code = src.Code;
                des.MainOwnerID = src.MainOwnerID;
                des.MainOwnerName = src.MainOwnerName;
                des.ProcurementPlanValue = src.ProcurementPlanValue;
                des.ProcurementRequirementValue = src.ProcurementRequirementValue;
                des.USDExchangeValue = src.USDExchangeValue;
                des.ContractorChoiceTypeID = src.ContractorChoiceTypeID;
                des.ContractorChoiceTypeName = src.ContractorChoiceTypeName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
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
        public bool Delete(ProcurementRequirement src)
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
