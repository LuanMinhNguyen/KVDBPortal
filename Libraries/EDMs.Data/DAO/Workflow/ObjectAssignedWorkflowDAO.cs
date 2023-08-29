// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectAssignedWorkflowDAO.cs" company="">
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
    public class ObjectAssignedWorkflowDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAssignedWorkflowDAO"/> class.
        /// </summary>
        public ObjectAssignedWorkflowDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ObjectAssignedWorkflow> GetIQueryable()
        {
            return EDMsDataContext.ObjectAssignedWorkflows;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ObjectAssignedWorkflow> GetAll()
        {
            return EDMsDataContext.ObjectAssignedWorkflows.OrderByDescending(t => t.ID).ToList();
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
        public ObjectAssignedWorkflow GetById(Guid id)
        {
            return EDMsDataContext.ObjectAssignedWorkflows.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(ObjectAssignedWorkflow ob)
        {
            try
            {
                EDMsDataContext.AddToObjectAssignedWorkflows(ob);
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
        public bool Update(ObjectAssignedWorkflow src)
        {
            try
            {
                ObjectAssignedWorkflow des = (from rs in EDMsDataContext.ObjectAssignedWorkflows
                                where rs.ID == src.ID
                                select rs).First();

                des.ObjectID = src.ObjectID;
                des.WorkflowID = src.WorkflowID;
                des.CurrentWorkflowStepID = src.CurrentWorkflowStepID;
                des.NextWorkflowStepID = src.NextWorkflowStepID;
                des.RejectWorkflowStepID = src.RejectWorkflowStepID;
                des.IsComplete = src.IsComplete;
                des.IsReject = src.IsReject;
                des.IsLeaf = src.IsLeaf;
                des.AssignedBy = src.AssignedBy;
                des.WorkflowName = src.WorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.NextWorkflowStepName = src.NextWorkflowStepName;
                des.RejectWorkflowStepName = src.RejectWorkflowStepName;
                des.CanReject = src.CanReject;
                des.RejectFromId = src.RejectFromId;
                des.ObjectWFDwtailId = src.ObjectWFDwtailId;
                des.PreviousStepId = src.PreviousStepId;
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
        public bool Delete(ObjectAssignedWorkflow src)
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
        public bool Delete(Guid ID)
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
