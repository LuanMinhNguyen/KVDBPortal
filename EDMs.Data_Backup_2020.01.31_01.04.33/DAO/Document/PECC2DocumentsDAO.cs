 

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PECC2ConrespondenceDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Telerik.Web.UI;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class PECC2DocumentsDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2ConrespondenceDAO"/> class.
        /// </summary>
        public PECC2DocumentsDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<PECC2Documents> GetIQueryable()
        {
            return this.EDMsDataContext.PECC2Documents;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PECC2Documents> GetAll()
        {
            return this.EDMsDataContext.PECC2Documents.OrderByDescending(t => t.ID).ToList();
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
        public PECC2Documents GetById(Guid id)
        {
            return this.EDMsDataContext.PECC2Documents.FirstOrDefault(ob => ob.ID == id);
        }

        #endregion

        #region GET ADVANCE
        public List<PECC2Documents> GetByProjectCode(int projectID)
        {
            return
                this.EDMsDataContext.PECC2Documents.Where(t => t.ProjectId == projectID).ToList();
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
        /// The <see cref="int?"/>.
        /// </returns>
        public Guid? Insert(PECC2Documents ob)
        {
            try
            {
                this.EDMsDataContext.AddToPECC2Documents(ob);
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
        public bool Update(PECC2Documents src)
        {
            try
            {
                PECC2Documents des = (from rs in this.EDMsDataContext.PECC2Documents
                                       where rs.ID == src.ID
                                       select rs).First();
                des.DocNo = src.DocNo;
                des.DocTitle = src.DocTitle;
                des.ConfidentialityId = src.ConfidentialityId;
                des.ConfidentialityName = src.ConfidentialityName;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.DocTypeCode = src.DocTypeCode;
                des.DocTypeId = src.DocTypeId;
                des.OriginatingOrganisationId = src.OriginatingOrganisationId;
                des.OriginatingOrganisationName = src.OriginatingOrganisationName;
                des.ReceivingOrganisationId = src.ReceivingOrganisationId;
                des.ReceivingOrganisationName = src.ReceivingOrganisationName;
                des.Year = src.Year;
                des.GroupId = src.GroupId;
                des.GroupCode = src.GroupCode;
                des.Date = src.Date;
                des.ResponseToId = src.ResponseToId;
                des.ResponseToName = src.ResponseToName;
                des.ResponseRequiredDate = src.ResponseRequiredDate;
                des.CarbonCopyId = src.CarbonCopyId;
                des.CarbonCopyName = src.CarbonCopyName;
                des.RelatedCSLId = src.RelatedCSLId;
                des.RelatedCSLNo = src.RelatedCSLNo;
                des.IsNeedReply = src.IsNeedReply;
                des.Description = src.Description;
                des.Treatment = src.Treatment;
                des.ProposedBy = src.ProposedBy;
                des.ProposedDate = src.ProposedDate;
                des.ReviewedBy = src.ReviewedBy;
                des.ReviewedDate = src.ReviewedDate;
                des.ApprovedBy = src.ApprovedBy;
                des.ApprovedDate = src.ApprovedDate;
                des.IssuedDateFrom = src.IssuedDateFrom;
                des.ReceivedDateTo = src.ReceivedDateTo;
                des.ReceivedDateCC = src.ReceivedDateCC;
                des.AreaCode = src.AreaCode;
                des.AreaId = src.AreaId;
                des.UnitId = src.UnitId;
                des.UnitCode = src.UnitCode;
                des.SystemCode = src.SystemCode;
                des.SystemId = src.SystemId;
                des.SubsystemCode = src.SubsystemCode;
                des.SubsystemId = src.SubsystemId;
                des.KKSId = src.KKSId;
                des.KKSCode = src.KKSCode;
                des.TrainNo = src.TrainNo;
                des.DisciplineCode = src.DisciplineCode;
                des.DisciplineId = src.DisciplineId;
                des.SheetNo = src.SheetNo;
                des.PlannedDate = src.PlannedDate;
                des.ActualDate = src.ActualDate;
                des.Remarks = src.Remarks;
                des.RevisionSchemaId = src.RevisionSchemaId;
                des.RevisionSchemaName = src.RevisionSchemaName;
                des.MajorRev = src.MajorRev;
                des.MinorRev = src.MinorRev;
                des.RevDate = src.RevDate;
                des.RevStatusId = src.RevStatusId;
                des.RevStatusName = src.RevStatusName;
                des.RevRemarks = src.RevRemarks;
                des.DocActionCode = src.DocActionCode;
                des.DocActionId = src.DocActionId;
                des.DocReviewStatusId = src.DocReviewStatusId;
                des.DocReviewStatusCode = src.DocReviewStatusCode;
                des.IsLeaf = src.IsLeaf;
                des.IsDelete = src.IsDelete;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.IncomingTransId = src.IncomingTransId;
                des.IncomingTransNo = src.IncomingTransNo;
                des.OutgoingTransId = src.OutgoingTransId;
                des.OutgoingTransNo = src.OutgoingTransNo;
                des.IsCreateOutgoingTrans = src.IsCreateOutgoingTrans;
                des.IsHasAttachFile = src.IsHasAttachFile;
                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.ManHour = src.ManHour;
                des.IsUseCustomWfFromTrans = src.IsUseCustomWfFromTrans;
                des.IsUseIsUseCustomWfFromObj = src.IsUseIsUseCustomWfFromObj;
                des.DocReviewStatusCode2 = src.DocReviewStatusCode2;
                des.DocReviewStatusId2 = src.DocReviewStatusId2;

                des.ChangeRequestId = src.ChangeRequestId;
                des.ChangeRequestNo = src.ChangeRequestNo;
                des.ChangeRequestIdFoRevised = src.ChangeRequestIdFoRevised;
                des.ChangeRequestNoFoRevised = src.ChangeRequestNoFoRevised;
                des.ChangeRequestReviewResultId = src.ChangeRequestReviewResultId;
                des.ChangeRequestReviewResultCode = src.ChangeRequestReviewResultCode;
                des.DocReviewStatusCode2 = src.DocReviewStatusCode2;
                des.DocReviewStatusId2 = src.DocReviewStatusId2;
                des.IsConsultantComment = src.IsConsultantComment;
                des.IsOwnerComment = src.IsOwnerComment;
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
        public bool Delete(PECC2Documents src)
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
