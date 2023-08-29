// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractorTransmittalDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class ContractorTransmittalDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractorTransmittalDAO"/> class.
        /// </summary>
        public ContractorTransmittalDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ContractorTransmittal> GetIQueryable()
        {
            return this.EDMsDataContext.ContractorTransmittals;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ContractorTransmittal> GetAll()
        {
            return this.EDMsDataContext.ContractorTransmittals.ToList();
        }

        /// <summary>
        /// The get all by owner.
        /// </summary>
        /// <param name="createdBy">
        /// The created by.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ContractorTransmittal> GetAllByOwner(int userId)
        {
            return this.EDMsDataContext.ContractorTransmittals.Where(t => t.CreatedBy == userId || t.FromId == userId || t.ToId == userId).ToList();
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
        public ContractorTransmittal GetById(Guid id)
        {
            return this.EDMsDataContext.ContractorTransmittals.FirstOrDefault(ob => ob.ID == id);
        }
       

        public ContractorTransmittal GetByDQRETransmittal(Guid ID)
        {
            return this.EDMsDataContext.ContractorTransmittals.FirstOrDefault(ob => ob.DQRETransId!= null && ob.DQRETransId == ID);
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
        public List<ContractorTransmittal> GetSpecific(Guid tranId)
        {
            return this.EDMsDataContext.ContractorTransmittals.Where(t => t.ID == tranId).ToList();
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
        public Guid? Insert(ContractorTransmittal ob)
        {
            try
            {
                this.EDMsDataContext.AddToContractorTransmittals(ob);
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
        public bool Update(ContractorTransmittal src)
        {
            try
            {
                ContractorTransmittal des = (from rs in this.EDMsDataContext.ContractorTransmittals
                                where rs.ID == src.ID
                                select rs).First();
                des.TransNo = src.TransNo;
                des.TransDate = src.TransDate;
                des.DueDate = src.DueDate;
                des.FromId = src.FromId;
                des.FromName = src.FromName;
                des.ToId = src.ToId;
                des.ToName = src.ToName;
                des.PurposeId = src.PurposeId;
                des.PurposeName = src.PurposeName;

                des.DQRETransId = src.DQRETransId;
                des.PECC2TransId = src.PECC2TransId;
                des.ReceivedDate = src.ReceivedDate;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.Status = src.Status;
                des.ErrorMessage = src.ErrorMessage;
                des.IsValid = src.IsValid;
                des.IsSend = src.IsSend;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedByName = src.LastUpdatedByName;

                des.IsOpen = src.IsOpen;

                des.RefTransId = src.RefTransId;
                des.RefTransNo = src.RefTransNo;
                des.GroupId = src.GroupId;
                des.GroupCode = src.GroupCode;
                des.FromValue = src.FromValue;
                des.ToValue = src.ToValue;
                des.CCValue = src.CCValue;
                des.Subject = src.Subject;
                des.Remark = src.Remark;
                des.TransmittedByName = src.TransmittedByName;
                des.TransmittedByDesignation = src.TransmittedByDesignation;
                des.AcknowledgedByDesignation = src.AcknowledgedByDesignation;
                des.AcknowledgedByName = src.AcknowledgedByName;
                des.Description = src.Description;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
                des.ReceivingOrganizationId = src.ReceivingOrganizationId;
                des.ReceivingOrganizationName = src.ReceivingOrganizationName;
                des.Sequence = src.Sequence;
                des.SequenceString = src.SequenceString;
                des.Year = src.Year;
                des.ForSentId = src.ForSentId;
                des.ForSentName = src.ForSentName;
                des.CCOrganizationId = src.CCOrganizationId;
                des.CCOrganizationName = src.CCOrganizationName;
                des.AcknowledgedId = src.AcknowledgedId;
                des.TransmittedById = src.TransmittedById;
                des.IsReject = src.IsReject;
                des.CurrentRejectReason = src.CurrentRejectReason;
                des.ChangeRequestId = src.ChangeRequestId;
                des.ChangeRequestNumber = src.ChangeRequestNumber;
                des.ConsultantDeadline = src.ConsultantDeadline;
                des.OwnerDeadline = src.OwnerDeadline;
                des.Priority = src.Priority;

                des.CategoryId = src.CategoryId;
                des.CategoryName = src.CategoryName;
                des.VolumeNumber = src.VolumeNumber;

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
        public bool Delete(ContractorTransmittal src)
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
        /// <param name="id"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(Guid id)
        {
            try
            {
                var des = this.GetById(id);
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
