using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.Workflow;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Workflow
{
   public  class TemplateWorkflowDetailService
    {/// <summary>
     /// The repo.
     /// </summary>
        private readonly TemplateWorkflowDetailDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="TemplateWorkflowDetailService"/> class.
        /// </summary>
        public TemplateWorkflowDetailService()
        {
            this.repo = new TemplateWorkflowDetailDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)
        public TemplateWorkflowDetail GetByCurrentStep(int wfStepId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.CurrentWorkflowStepID == wfStepId);
        }
        public TemplateWorkflowDetail GetByCurrentStep(int wfStepId, int usercreate)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.CurrentWorkflowStepID == wfStepId && t.CreatedBy==usercreate);
        }
        public List<TemplateWorkflowDetail> GetAllByWorkflow(int wfId)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == wfId).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
        public List<TemplateWorkflowDetail> GetAllByWorkflow(int wfId, int userCreate)
        {
            return this.repo.GetAll().Where(t => t.WorkflowID == wfId && t.CreatedBy==userCreate).OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<TemplateWorkflowDetail> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.CurrentWorkflowStepName).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public TemplateWorkflowDetail GetById(int id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(TemplateWorkflowDetail bo)
        {
            var objId = this.repo.Insert(bo);
            // Trigger data change
            //if (objId != null)
            //{
            //    var changeData = new WaitingSyncData()
            //    {
            //        ActionTypeID = 1,
            //        ActionTypeName = "Insert",
            //        ObjectID2 = objId,
            //        ObjectName = "[WMS].[TemplateWorkflowDetails]",
            //        EffectDate = DateTime.Now,
            //        IsSynced = false
            //    };

            //    this.waitingSyncDataService.Insert(changeData);
            //}

            // -------------------------------------------------------

            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(TemplateWorkflowDetail bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                //if (flag)
                //{
                //    var changeData = new WaitingSyncData()
                //    {
                //        ActionTypeID = 2,
                //        ActionTypeName = "Update",
                //        ObjectID2 = bo.ID,
                //        ObjectName = "[WMS].[TemplateWorkflowDetails]",
                //        EffectDate = DateTime.Now,
                //        IsSynced = false
                //    };

                //    this.waitingSyncDataService.Insert(changeData);
                //}

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(TemplateWorkflowDetail bo)
        {
            try
            {
                return this.repo.Delete(bo);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Resource By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var flag = this.repo.Delete(id);
                //if (flag)
                //{
                //    // Trigger data change
                //    var changeData = new WaitingSyncData()
                //    {
                //        ActionTypeID = 3,
                //        ActionTypeName = "Delete",
                //        ObjectID2 = id,
                //        ObjectName = "[WMS].[TemplateWorkflowDetails]",
                //        EffectDate = DateTime.Now,
                //        IsSynced = false
                //    };

                //    this.waitingSyncDataService.Insert(changeData);
                //    // ----------------------------------------------------------------------
                //}

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
