// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractDAO.cs" company="">
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
    public class ContractDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractDAO"/> class.
        /// </summary>
        public ContractDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Contract> GetIQueryable()
        {
            return this.EDMsDataContext.Contracts;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Contract> GetAll()
        {
            return this.EDMsDataContext.Contracts.OrderByDescending(t => t.ID).ToList();
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
        public Contract GetById(int id)
        {
            return this.EDMsDataContext.Contracts.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(Contract ob)
        {
            try
            {
                this.EDMsDataContext.AddToContracts(ob);
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
        public bool Update(Contract src)
        {
            try
            {
                Contract des = (from rs in this.EDMsDataContext.Contracts
                                where rs.ID == src.ID
                                select rs).First();

                des.Number = src.Number;
                des.Note = src.Note;
                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
                des.ProcurementRequirementID = src.ProcurementRequirementID;
                des.ProcurementRequirementNumber = src.ProcurementRequirementNumber;
                des.Number = src.Number;
                des.ContractorSelectedID = src.ContractorSelectedID;
                des.ContractorSelectedName = src.ContractorSelectedName;
                des.ContractContent = src.ContractContent;
                des.EffectedDate = src.EffectedDate;
                des.EndDate = src.EndDate;
                des.DeliveryDate = src.DeliveryDate;
                des.DeliveryStatus = src.DeliveryStatus;
                des.ContractTypeID = src.ContractTypeID;
                des.ContractTypeName = src.ContractTypeName;
                des.ContractStatusID = src.ContractStatusID;
                des.ContractStausName = src.ContractStausName;
                des.ExchangeRate = src.ExchangeRate;
                des.ContractValueUSD = src.ContractValueUSD;
                des.ContractValueVND = src.ContractValueVND;
                des.ArisingTotalValue = src.ArisingTotalValue;
                des.ContractTotalValue = src.ContractTotalValue;
                des.PaymentedValueUSD = src.PaymentedValueUSD;
                des.PaymentedValueVND = src.PaymentedValueVND;
                des.RemainPaymentUSD = src.RemainPaymentUSD;
                des.RemainPaymentVND = src.RemainPaymentVND;
                des.DeferenceWithPRValue = src.DeferenceWithPRValue;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;

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
        public bool Delete(Contract src)
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
