// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeRequestDAO.cs" company="">
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
    public class ChangeRequestDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRequestDAO"/> class.
        /// </summary>
        public ChangeRequestDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ChangeRequest> GetIQueryable()
        {
            return this.EDMsDataContext.ChangeRequests;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ChangeRequest> GetAll()
        {
            return this.EDMsDataContext.ChangeRequests.ToList();
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
        public ChangeRequest GetById(Guid id)
        {
            return this.EDMsDataContext.ChangeRequests.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(ChangeRequest ob)
        {
            try
            {
                this.EDMsDataContext.AddToChangeRequests(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch (Exception exception)
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
        public bool Update(ChangeRequest src)
        {
            try
            {
                ChangeRequest des = (from rs in this.EDMsDataContext.ChangeRequests
                                where rs.ID == src.ID
                                select rs).First();

                des.Number = src.Number;
                des.Description = src.Description;
                des.ConfidentialityId = src.ConfidentialityId;
                des.ConfidentialityName = src.ConfidentialityName;
                des.AreaId = src.AreaId;
                des.AreaCode = src.AreaCode;
                des.UnitCode = src.UnitCode;
                des.UnitId = src.UnitId;
                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;
                des.GroupId = src.GroupId;
                des.GroupName = src.GroupName;
                des.Year = src.Year;
                des.SequentialNumber = src.SequentialNumber;
                des.ReasonForChange = src.ReasonForChange;
                des.ExistingCondition = src.ExistingCondition;
                des.IssuedDate = src.IssuedDate;
                des.ClosedDate = src.ClosedDate;
                des.ChangeGradeCodeId = src.ChangeGradeCodeId;
                des.ChangeGradeCodeName = src.ChangeGradeCodeName;
                des.Status = src.Status;
                des.RefDocId = src.RefDocId;
                des.RefDocNo = src.RefDocNo;
                des.ReviewResultId = src.ReviewResultId;
                des.ReviewResultName = src.ReviewResultName;
                des.PECC2ReviewResultId = src.PECC2ReviewResultId;
                des.PECC2ReviewResultName = src.PECC2ReviewResultName;
                des.OwnerReviewResultId = src.OwnerReviewResultId;
                des.OwnerReviewResultName = src.OwnerReviewResultName;
                des.DocToBeRevisedId = src.DocToBeRevisedId;
                des.IsDelete = src.IsDelete;

                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;

                des.IncomingTransId = src.IncomingTransId;
                des.IncomingTransNo = src.IncomingTransNo;
                des.OutgoingTransId = src.OutgoingTransId;
                des.OutgoingTransNo = src.OutgoingTransNo;
                des.IsCreateOutgoingTrans = src.IsCreateOutgoingTrans;
                des.IsHasAttachFile = src.IsHasAttachFile;

                des.IsUseCustomWfFromTrans = src.IsUseCustomWfFromTrans;
                des.IsUseIsUseCustomWfFromObj = src.IsUseIsUseCustomWfFromObj;

                des.ErrorMessage = src.ErrorMessage;
                des.IsSend = src.IsSend;
                des.IsValid = src.IsValid;
                des.SendDate = src.SendDate;
                des.SendById = src.SendById;
                des.SendByName = src.SendByName;
                des.FromOrganizationId = src.FromOrganizationId;
                des.IsAttachWorkflow = src.IsAttachWorkflow;
                des.RefChangeRequestId = src.RefChangeRequestId;
                des.RefChangeRequestNumber = src.RefChangeRequestNumber;
                des.IsReject = src.IsReject;
                des.IsFirst = src.IsFirst;

                des.IsLeaf = src.IsLeaf;
                des.Revision = src.Revision;
                des.ParentId = src.ParentId;
                des.Title = src.Title;
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
        public bool Delete(ChangeRequest src)
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
