// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialRequisitionDetailDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.WMS
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class MaterialRequisitionDetailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRequisitionDetailDAO"/> class.
        /// </summary>
        public MaterialRequisitionDetailDAO() : base()
        {
        }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<MaterialRequisitionDetail> GetIQueryable()
        {
            return this.EDMsDataContext.MaterialRequisitionDetails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<MaterialRequisitionDetail> GetAll()
        {
            return this.EDMsDataContext.MaterialRequisitionDetails.OrderByDescending(t => t.ID).ToList();
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
        public MaterialRequisitionDetail GetById(Guid id)
        {
            return this.EDMsDataContext.MaterialRequisitionDetails.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(MaterialRequisitionDetail ob)
        {
            try
            {
                this.EDMsDataContext.AddToMaterialRequisitionDetails(ob);
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
        public bool Update(MaterialRequisitionDetail src)
        {
            try
            {
                MaterialRequisitionDetail des = (from rs in this.EDMsDataContext.MaterialRequisitionDetails
                    where rs.ID == src.ID
                    select rs).First();

                des.MRId = src.MRId;
                des.MRNo = src.MRNo;
                des.ReqROBMax = src.ReqROBMax;
                des.ReqROBMin = src.ReqROBMin;
                des.ROB = src.ROB;
                des.QtyRemarkForSpare = src.QtyRemarkForSpare;
                des.QtyRemarkUseForJob = src.QtyRemarkUseForJob;
                des.QtyReq = src.QtyReq;
                des.Units = src.Units;
                des.SFICode = src.SFICode;
                des.Description = src.Description;
                des.MakerName = src.MakerName;
                des.CertificateRequired = src.CertificateRequired;
                des.Alternative = src.Alternative;
                des.NormalUsingFrequency = src.NormalUsingFrequency;
                des.Remarks = src.Remarks;
                des.IsLeaf = src.IsLeaf;
                des.IsCancel = src.IsCancel;
                des.ParentId = src.ParentId;
                des.CreatedBy = src.CreatedBy;
                des.CreatedByName = src.CreatedByName;
                des.CreatedDate = src.CreatedDate;

                des.PriorityName = src.PriorityName;
                des.MRRecieveDate = src.MRRecieveDate;
                des.DepartmentId = src.DepartmentId;
                des.DepartmentName = src.DepartmentName;
                des.PICIds = src.PICIds;
                des.PICName = src.PICName;
                des.ContractNumber = src.ContractNumber;
                des.ReqQuotationDate = src.ReqQuotationDate;
                des.ReceiveQuotationDate = src.ReceiveQuotationDate;
                des.PONumber = src.PONumber;
                des.POIssueDate = src.POIssueDate;
                des.UnitPrice = src.UnitPrice;
                des.ExpectedDeliveryDate = src.ExpectedDeliveryDate;
                des.DeliveryDateRevisedBySupplier = src.DeliveryDateRevisedBySupplier;
                des.ActualDeliveryDate = src.ActualDeliveryDate;
                des.SupplierName = src.SupplierName;
                des.MRPurchasingStatus = src.MRPurchasingStatus;
                des.MRDetailPurchasingStatus = src.MRDetailPurchasingStatus;


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
        public bool Delete(MaterialRequisitionDetail src)
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
