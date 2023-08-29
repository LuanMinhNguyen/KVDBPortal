// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PECC2TransmittalDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class PECC2TransmittalDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2TransmittalDAO"/> class.
        /// </summary>
        public PECC2TransmittalDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<PECC2Transmittal> GetIQueryable()
        {
            return this.EDMsDataContext.PECC2Transmittal;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PECC2Transmittal> GetAll()
        {
            return this.EDMsDataContext.PECC2Transmittal.ToList();
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
        //public List<PECC2Transmittal> GetAllByOwner(int userId)
        //{
        //    return this.EDMsDataContext.PECC2Transmittals.Where(t => t.CreatedBy == userId || t.FromId == userId || t.ToId == userId).ToList();
        //}

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public PECC2Transmittal GetById(Guid id)
        {
            return this.EDMsDataContext.PECC2Transmittal.FirstOrDefault(ob => ob.ID == id);
        }
        public PECC2Transmittal GetByRefId(Guid id)
        {
            return this.EDMsDataContext.PECC2Transmittal.FirstOrDefault(ob => ob.RefTransId == id);
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
        //public List<PECC2Transmittal> GetSpecific(int tranId)
        //{
        //    return this.EDMsDataContext.PECC2Transmittals.Where(t => t.ID == tranId).ToList();
        //}
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
        public Guid? Insert(PECC2Transmittal ob)
        {
            try
            {
                this.EDMsDataContext.AddToPECC2Transmittal(ob);
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
        public bool Update(PECC2Transmittal src)
        {
            try
            {
                PECC2Transmittal des = (from rs in this.EDMsDataContext.PECC2Transmittal
                                where rs.ID == src.ID
                                select rs).First();

                des.TransmittalNo = src.TransmittalNo;
                des.Description = src.Description;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
                des.ReceivingOrganizationId = src.ReceivingOrganizationId;
                des.ReceivingOrganizationName = src.ReceivingOrganizationName;
                des.ProjectCodeId = src.ProjectCodeId;
                des.ProjectCodeName = src.ProjectCodeName;
                des.IssuedDate = src.IssuedDate;
                des.ReceivedDate = src.ReceivedDate;
                des.Remark = src.Remark;
                des.ConfidentialityId = src.ConfidentialityId;
                des.ConfidentialityName = src.ConfidentialityName;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;
                des.File = src.File;
                des.HasAttachFile = src.HasAttachFile;
                des.CreatedBy = src.CreatedBy;
                des.CreatedDate = src.CreatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;

                des.TypeId = src.TypeId;
                des.StoreFolderPath = src.StoreFolderPath;
                des.Status = src.Status;
                des.IsOpen = src.IsOpen;
                des.IsImport = src.IsImport;
                des.ContractorTransId = src.ContractorTransId;
                des.IsSend = src.IsSend;
                des.ErrorMessage = src.ErrorMessage;
                des.IsValid = src.IsValid;
                des.DueDate = src.DueDate;

                des.RefTransId = src.RefTransId;
                des.RefTransNo = src.RefTransNo;
                des.IsAttachWorkflow = src.IsAttachWorkflow;

                des.GroupId = src.GroupId;
                des.GroupCode = src.GroupCode;
                des.OtherTransNo = src.OtherTransNo;
                des.FromValue = src.FromValue;
                des.ToValue = src.ToValue;
                des.CCValue = src.CCValue;
                des.Subject = src.Subject;
                des.TransmittedByName = src.TransmittedByName;
                des.TransmittedByDesignation = src.TransmittedByDesignation;
                des.AcknowledgedByDesignation = src.AcknowledgedByDesignation;
                des.AcknowledgedByName = src.AcknowledgedByName;

                des.Sequence = src.Sequence;
                des.SequenceString = src.SequenceString;
                des.Year = src.Year;
                des.ForSentName = src.ForSentName;
                des.ForSentId = src.ForSentId;
                des.CCOrganizationId = src.CCOrganizationId;
                des.CCOrganizationName = src.CCOrganizationName;
                des.IsAllDocCompleteWorkflow = src.IsAllDocCompleteWorkflow;
                des.IsCreateOutGoingTrans = src.IsCreateOutGoingTrans;
                des.AcknowledgedId = src.AcknowledgedId;
                des.TransmittedById = src.TransmittedById;

                des.PurposeId = src.PurposeId;
                des.PurposeName = src.PurposeName;

                des.IsReject = src.IsReject;
                des.CurrentRejectReason = src.CurrentRejectReason;

                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.IsUseCustomWf = src.IsUseCustomWf;

                des.StoreFolderPathContractor = src.StoreFolderPathContractor;

                des.IsFirstTrans = src.IsFirstTrans;
                des.ChangeRequestId = src.ChangeRequestId;
                des.ChangeRequestNumber = src.ChangeRequestNumber;
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
        public bool Delete(PECC2Transmittal src)
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
