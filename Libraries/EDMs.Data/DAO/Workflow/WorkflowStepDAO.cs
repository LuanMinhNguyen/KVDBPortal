// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WorkflowStepDAO.cs" company="">
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
    public class WorkflowStepDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WorkflowStepDAO"/> class.
        /// </summary>
        public WorkflowStepDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Entities.WorkflowStep> GetIQueryable()
        {
            return this.EDMsDataContext.WorkflowSteps;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Entities.WorkflowStep> GetAll()
        {
            return this.EDMsDataContext.WorkflowSteps.OrderByDescending(t => t.ID).ToList();
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
        public Entities.WorkflowStep GetById(int id)
        {
            return this.EDMsDataContext.WorkflowSteps.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(Entities.WorkflowStep ob)
        {
            try
            {
                this.EDMsDataContext.AddToWorkflowSteps(ob);
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
        public bool Update(Entities.WorkflowStep src)
        {
            try
            {
                Entities.WorkflowStep des = (from rs in this.EDMsDataContext.WorkflowSteps
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.IsFirst = src.IsFirst;
                des.IsCreated = src.IsCreated;
                des.CanReject = src.CanReject;
                des.IsCanCreateOutgoingTrans = src.IsCanCreateOutgoingTrans;
                des.ActionApplyCode = src.ActionApplyCode;
                des.ActionApplyName = src.ActionApplyName;
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
        public bool Delete(Entities.WorkflowStep src)
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
