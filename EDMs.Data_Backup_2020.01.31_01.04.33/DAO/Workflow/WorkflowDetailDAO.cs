// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkflowDetailDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.Workflow
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class WorkflowDetailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowDetailDAO"/> class.
        /// </summary>
        public WorkflowDetailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<WorkflowDetail> GetIQueryable()
        {
            return EDMsDataContext.WorkflowDetails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<WorkflowDetail> GetAll()
        {
            return EDMsDataContext.WorkflowDetails.OrderByDescending(t => t.ID).ToList();
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
        public WorkflowDetail GetById(int id)
        {
            return EDMsDataContext.WorkflowDetails.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(WorkflowDetail ob)
        {
            try
            {
                EDMsDataContext.AddToWorkflowDetails(ob);
                EDMsDataContext.SaveChanges();
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
        public bool Update(WorkflowDetail src)
        {
            try
            {
                WorkflowDetail des = (from rs in EDMsDataContext.WorkflowDetails
                                where rs.ID == src.ID
                                select rs).First();
                des.WorkflowID = src.WorkflowID;
                des.WorkflowName = src.WorkflowName;
                des.CurrentWorkflowStepID = src.CurrentWorkflowStepID;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.StepDefinitionID = src.StepDefinitionID;
                des.StepDefinitionName = src.StepDefinitionName;
                des.Duration = src.Duration;
                des.AssignUserIDs = src.AssignUserIDs;
                des.InformationOnlyUserIDs = src.InformationOnlyUserIDs;
                des.DistributionMatrixIDs = src.DistributionMatrixIDs;
                des.NextWorkflowStepID = src.NextWorkflowStepID;
                des.NextWorkflowStepName = src.NextWorkflowStepName;
                des.RejectWorkflowStepID = src.RejectWorkflowStepID;
                des.RejectWorkflowStepName = src.RejectWorkflowStepName;

                des.AssignRoleIDs = src.AssignRoleIDs;
                des.InformationOnlyRoleIDs = src.InformationOnlyRoleIDs;
                des.Recipients = src.Recipients;

                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.AssignTitleIds = src.AssignTitleIds;
                des.ManagementUserIds = src.ManagementUserIds;
                des.ApproveUserIds = src.ApproveUserIds;
                des.ReviewUserIds = src.ReviewUserIds;
                des.CommentUserIds = src.CommentUserIds;
                des.ActionApplyCode = src.ActionApplyCode;
                des.ActionApplyName = src.ActionApplyName;
                des.ConsolidateUserIds = src.ConsolidateUserIds;
                des.IsFirst = src.IsFirst;
                des.CanReject = src.CanReject;
                des.IsCreated = src.IsCreated;
                EDMsDataContext.SaveChanges();
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
        public bool Delete(WorkflowDetail src)
        {
            try
            {
                var des = GetById(src.ID);
                if (des != null)
                {
                    EDMsDataContext.DeleteObject(des);
                    EDMsDataContext.SaveChanges();
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
                var des = GetById(ID);
                if (des != null)
                {
                    EDMsDataContext.DeleteObject(des);
                    EDMsDataContext.SaveChanges();
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
