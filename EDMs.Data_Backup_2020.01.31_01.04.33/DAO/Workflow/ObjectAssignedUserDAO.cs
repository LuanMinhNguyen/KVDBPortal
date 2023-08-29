// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectAssignedUserDAO.cs" company="">
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

namespace EDMs.Data.DAO.Workflow
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class ObjectAssignedUserDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAssignedUserDAO"/> class.
        /// </summary>
        public ObjectAssignedUserDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Entities.ObjectAssignedUser> GetIQueryable()
        {
            return this.EDMsDataContext.ObjectAssignedUsers;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Entities.ObjectAssignedUser> GetAll()
        {
            return this.EDMsDataContext.ObjectAssignedUsers.OrderByDescending(t => t.ID).ToList();
        }
        public List<Entities.ObjectAssignedUser> GetAllWFISAdmin()
        {
            return this.EDMsDataContext.ObjectAssignedUsers.Where(t=> !t.IsComplete.GetValueOrDefault() ).ToList();
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
        public Entities.ObjectAssignedUser GetById(Guid id)
        {
            return this.EDMsDataContext.ObjectAssignedUsers.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(Entities.ObjectAssignedUser ob)
        {
            try
            {
                this.EDMsDataContext.AddToObjectAssignedUsers(ob);
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
        public bool Update(Entities.ObjectAssignedUser src)
        {
            try
            {
                Entities.ObjectAssignedUser des = (from rs in this.EDMsDataContext.ObjectAssignedUsers
                                where rs.ID == src.ID
                                select rs).First();

                des.ObjectAssignedWorkflowID = src.ObjectAssignedWorkflowID;
                des.ObjectID = src.ObjectID;
                des.UserID = src.UserID;
                des.ReceivedDate = src.ReceivedDate;
                des.PlanCompleteDate = src.PlanCompleteDate;
                des.IsOverDue = src.IsOverDue;
                des.IsComplete = src.IsComplete;
                des.AssignedBy = src.AssignedBy;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentWorkflowStepId = src.CurrentWorkflowStepId;
                des.IsReject = src.IsReject;
                des.RejectFromId = src.RejectFromId;
                des.IsFinal = src.IsFinal;
                des.CommentContent = src.CommentContent;
                des.ActualDate = src.ActualDate;

                des.FinalAssignDeptId = src.FinalAssignDeptId;
                des.FinalAssignDeptName = src.FinalAssignDeptName;
                des.UserFullName = src.UserFullName;
                des.ActionTypeName = src.ActionTypeName;
                des.ActionTypeId = src.ActionTypeId;
                des.Status = src.Status;
                des.WorkingStatus = src.WorkingStatus;
                des.IsMainWorkflow = src.IsMainWorkflow;
                des.IsAddAnotherDisciplineLead = src.IsAddAnotherDisciplineLead;
                des.Revision = src.Revision;
                des.IsReassign = src.IsReassign;
                des.IsCanCreateOutgoingTrans = src.IsCanCreateOutgoingTrans;
                des.ObjectTypeId = src.ObjectTypeId;
                des.ObjectProjectId = src.ObjectProjectId;
                des.Categoryid = src.Categoryid;
                des.IsCompleteReject = src.IsCompleteReject;
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
        public bool Delete(Entities.ObjectAssignedUser src)
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
