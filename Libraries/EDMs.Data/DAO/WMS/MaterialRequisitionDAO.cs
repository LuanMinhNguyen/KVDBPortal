// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialRequisitionDAO.cs" company="">
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
    public class MaterialRequisitionDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialRequisitionDAO"/> class.
        /// </summary>
        public MaterialRequisitionDAO() : base()
        {
        }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<MaterialRequisition> GetIQueryable()
        {
            return this.EDMsDataContext.MaterialRequisitions;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<MaterialRequisition> GetAll()
        {
            return this.EDMsDataContext.MaterialRequisitions.OrderByDescending(t => t.ID).ToList();
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
        public MaterialRequisition GetById(Guid id)
        {
            return this.EDMsDataContext.MaterialRequisitions.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(MaterialRequisition ob)
        {
            try
            {
                this.EDMsDataContext.AddToMaterialRequisitions(ob);
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
        public bool Update(MaterialRequisition src)
        {
            try
            {
                MaterialRequisition des = (from rs in this.EDMsDataContext.MaterialRequisitions
                    where rs.ID == src.ID
                    select rs).First();

                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.AMOSWorkOrder = src.AMOSWorkOrder;
                des.MRNo = src.MRNo;
                des.DeparmentId = src.DeparmentId;
                des.DepartmentName = src.DepartmentName;
                des.Justification = src.Justification;
                des.MRTypeIds = src.MRTypeIds;
                des.MRTypeName = src.MRTypeName;
                des.DateRequire = src.DateRequire;
                des.PriorityId = src.PriorityId;
                des.PriorityName = src.PriorityName;
                des.OriginatorId = src.OriginatorId;
                des.OriginatorName = src.OriginatorName;
                des.OriginatorDate = src.OriginatorDate;
                des.StoreManId = src.StoreManId;
                des.StoreManDate = src.StoreManDate;
                des.StoreManName = src.StoreManName;
                des.SupervisorDate = src.SupervisorDate;
                des.SupervisorId = src.SupervisorId;
                des.SupervisorName = src.SupervisorName;
                des.OIMDate = src.OIMDate;
                des.OIMId = src.OIMId;
                des.OIMName = src.OIMName;

                des.Comment_PurchasingGroup = src.Comment_PurchasingGroup;
                des.Comment_ReceivedMRFromFacility = src.Comment_ReceivedMRFromFacility;
                des.Comment_MRProcessingCompleted = src.Comment_MRProcessingCompleted;

                des.Comment_ForwardMRToPurchasingGroup = src.Comment_ForwardMRToPurchasingGroup;
                des.Comment_ReceivedMRFromTech = src.Comment_ReceivedMRFromTech;
                des.Comment_ForwardMRToTechDept = src.Comment_ForwardMRToTechDept;

                des.Comment_OperationSign = src.Comment_OperationSign;
                des.Comment_OperationSignDate = src.Comment_OperationSignDate;
                des.Comment_OperationSignName = src.Comment_OperationSignName;

                des.Comment_TechSign = src.Comment_TechSign;
                des.Comment_TechSignDate = src.Comment_TechSignDate;
                des.Comment_TechSignName = src.Comment_TechSignName;

                des.Comment_DirectorSign = src.Comment_DirectorSign;
                des.Comment_DirectorSignDate = src.Comment_DirectorSignDate;
                des.Comment_DirectorSignName = src.Comment_DirectorSignName;

                des.UpdatedByDate = src.UpdatedByDate;
                des.UpdatedByName = src.UpdatedByName;
                des.UpdatedBy = src.UpdatedBy;

                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;

                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.IsCompleteFinal = src.IsCompleteFinal;

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
        public bool Delete(MaterialRequisition src)
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
