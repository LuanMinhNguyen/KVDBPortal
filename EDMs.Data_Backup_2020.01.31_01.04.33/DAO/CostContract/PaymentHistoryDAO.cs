// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentHistoryDAO.cs" company="">
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
    public class PaymentHistoryDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PaymentHistoryDAO"/> class.
        /// </summary>
        public PaymentHistoryDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<PaymentHistory> GetIQueryable()
        {
            return this.EDMsDataContext.PaymentHistories;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PaymentHistory> GetAll()
        {
            return this.EDMsDataContext.PaymentHistories.OrderByDescending(t => t.ID).ToList();
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
        public PaymentHistory GetById(int id)
        {
            return this.EDMsDataContext.PaymentHistories.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(PaymentHistory ob)
        {
            try
            {
                this.EDMsDataContext.AddToPaymentHistories(ob);
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
        public bool Update(PaymentHistory src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.PaymentHistories
                                where rs.ID == src.ID
                                select rs).First();
                des.ContractID = src.ContractID;
                des.ContractName = src.ContractName;
                des.ActualDate = src.ActualDate;
                des.ActualValueVND = src.ActualValueVND;
                des.ActualValueUSD = src.ActualValueUSD;
                des.PlanDate = src.PlanDate;
                des.PlanValueVND = src.PlanValueVND;
                des.PlanValueUSD = src.PlanValueUSD;
                des.Name = src.Name;

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
        public bool Delete(PaymentHistory src)
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
