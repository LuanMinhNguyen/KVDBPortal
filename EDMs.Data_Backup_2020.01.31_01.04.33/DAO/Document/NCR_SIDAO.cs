// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NCR_SIDAO.cs" company="">
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
    public class NCR_SIDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NCR_SIDAO"/> class.
        /// </summary>
        public NCR_SIDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<NCR_SI> GetIQueryable()
        {
            return this.EDMsDataContext.NCR_SI;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<NCR_SI> GetAll()
        {
            return this.EDMsDataContext.NCR_SI.ToList();
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
        public NCR_SI GetById(Guid id)
        {
            return this.EDMsDataContext.NCR_SI.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(NCR_SI ob)
        {
            try
            {
                this.EDMsDataContext.AddToNCR_SI(ob);
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
        public bool Update(NCR_SI src)
        {
            try
            {
                NCR_SI des = (from rs in this.EDMsDataContext.NCR_SI
                                where rs.ID == src.ID
                                select rs).First();

                des.Number = src.Number;
                des.Subject = src.Subject;
                des.ConfidentialityId = src.ConfidentialityId;
                des.ConfidentialityName = src.ConfidentialityName;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.GroupId = src.GroupId;
                des.GroupName = src.GroupName;
                des.OriginatingOrganisationOwnerId = src.OriginatingOrganisationOwnerId;
                des.OriginatingOrganisationOwnerName = src.OriginatingOrganisationOwnerName;
                des.OriginatingOrganisationPMCId = src.OriginatingOrganisationPMCId;
                des.OriginatingOrganisationPMCName = src.OriginatingOrganisationPMCName;
                des.ReceivingOrganisationEPCId = src.ReceivingOrganisationEPCId;
                des.ReceivingOrganisationEPCName = src.ReceivingOrganisationEPCName;
                des.RefDocId = src.RefDocId;
                des.RefDocNo = src.RefDocNo;
                des.Description = src.Description;
                des.ActionTake = src.ActionTake;
                des.IssuedDate = src.IssuedDate;
                des.IssuedByOwner = src.IssuedByOwner;
                des.IssuedByPMC = src.IssuedByPMC;
                des.ReceivedByEPC = src.ReceivedByEPC;
                des.ClosedDate = src.ClosedDate;
                des.Status = src.Status;
                des.ClosedByPMB = src.ClosedByPMB;
                des.ClosedByPMC = src.ClosedByPMC;
                des.IsDelete = src.IsDelete;
                des.Type = src.Type;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.IsHasAttachFile = src.IsHasAttachFile;

                des.IsUseCustomWfFromTrans = src.IsUseCustomWfFromTrans;
                des.IsUseIsUseCustomWfFromObj = src.IsUseIsUseCustomWfFromObj;

                des.SignedByPMC = src.SignedByPMC;
                des.SignedByPMB = src.SignedByPMB;
                des.DateOfSubmission = src.DateOfSubmission;
                des.Note = src.Note;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
                des.ReceivingOrganizationId = src.ReceivingOrganizationId;
                des.ReceivingOrganizationName = src.ReceivingOrganizationName;
                des.CCId = src.CCId;
                des.CCName = src.CCName;
                des.RelatedCSId = src.RelatedCSId;
                des.RelatedCSNo = src.RelatedCSNo;
                des.NeedReply = src.NeedReply;
                des.Treatment = src.Treatment;
                des.TypeName = src.TypeName;
                des.Reference = src.Reference;
                des.Year = src.Year;
                des.ReviewResult = src.ReviewResult;
                des.IsAttachWorkflow = src.IsAttachWorkflow;
                des.EPCUpdateActionTaken = src.EPCUpdateActionTaken;
                des.DisciplineID = src.DisciplineID;
                des.DisCiplineName = src.DisCiplineName;
                des.IsCancel = src.IsCancel;
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
        public bool Delete(NCR_SI src)
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
