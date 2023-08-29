// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingECRDAO.cs" company="">
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
    public class TrackingECRDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingECRDAO"/> class.
        /// </summary>
        public TrackingECRDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingECR> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingECRs;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingECR> GetAll()
        {
            return this.EDMsDataContext.TrackingECRs.OrderByDescending(t => t.ID).ToList();
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
        public TrackingECR GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingECRs.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(TrackingECR ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingECRs(ob);
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
        public bool Update(TrackingECR src)
        {
            try
            {
                TrackingECR des = (from rs in this.EDMsDataContext.TrackingECRs
                                where rs.ID == src.ID
                                select rs).First();


                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.Title = src.Title;
                des.Description = src.Description;
                des.DateRaised = src.DateRaised;

                des.Section1Id = src.Section1Id;
                des.Section1Name = src.Section1Name;
                des.Section2Id = src.Section2Id;
                des.Section2Name = src.Section2Name;
                des.Section3Id = src.Section3Id;
                des.Section3Name = src.Section3Name;
                des.Section4Id = src.Section4Id;
                des.Section4Name = src.Section4Name;
                des.Section5Id = src.Section5Id;
                des.Section5Name = src.Section5Name;

                des.ApSection3Id = src.ApSection3Id;
                des.ApSection3Name = src.ApSection3Name;
                des.ApRequirementId = src.ApRequirementId;
                des.ApRequirementName = src.ApRequirementName;
                des.ExecutionStatus = src.ExecutionStatus;
                des.PersonInChargeIds = src.PersonInChargeIds;
                des.PersonInCharge = src.PersonInCharge;
                des.Cost = src.Cost;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;
                des.Remark = src.Remark;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedDate = src.UpdatedDate;

                des.ParentId = src.ParentId;
                des.IsComplete = src.IsComplete;
                des.IsLeaf = src.IsLeaf;

                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;

                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.IsCompleteFinal = src.IsCompleteFinal;

                des.PriorityId = src.PriorityId;
                des.PriorityName = src.PriorityName;
                des.FinalAssignDeptName = src.FinalAssignDeptName;
                des.FinalAssignDeptId = src.FinalAssignDeptId;
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
        public bool Delete(TrackingECR src)
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
