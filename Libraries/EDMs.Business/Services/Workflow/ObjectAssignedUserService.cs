using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.Workflow;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Workflow
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class ObjectAssignedUserService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ObjectAssignedUserDAO repo;
        private readonly WaitingSyncDataService waitingSyncDataService;


        /// <summary>
        /// Initializes a new instance of the <see cref="ObjectAssignedUserService"/> class.
        /// </summary>
        public ObjectAssignedUserService()
        {
            this.repo = new ObjectAssignedUserDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();

        }

        #region Get (Advances)

        public List<Data.Entities.ObjectAssignedUser> GetSpecialObjectAssignedUsers(List<Guid> ids)
        {
            return this.repo.GetAll().Where(t => ids.Contains(t.ID)).ToList();
        }

        public List<Data.Entities.ObjectAssignedUser> GetAllIncompleteByUser(int userid)
        {
            return this.repo.GetAll().Where(t => t.UserID == userid 
                                                && !t.IsComplete.GetValueOrDefault()).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllIncompleteByAdmin()
        {
            return this.repo.GetAll().Where(t => !t.IsComplete.GetValueOrDefault()).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllByUser(int userid, string objType)
        {
            return this.repo.GetAll().Where(t => t.UserID == userid && t.ObjectType == objType && (t.ActionTypeId == null || t.ActionTypeId == 1)).ToList();
        }


        public List<Data.Entities.ObjectAssignedUser> GetAllIncompleteByDoc(Guid objId, int objType)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && (t.ActionTypeId == null || t.ActionTypeId == 1)
                                                && t.ObjectTypeId == objType).ToList();

        }


        public bool IsHaveRejectTask(Guid objId, int currentWorkflowStepId)
        {
            return this.repo.GetAll().Any(t => t.ObjectID == objId
                                               && t.CurrentWorkflowStepId == currentWorkflowStepId
                                               && t.IsCompleteReject.GetValueOrDefault()
                                               && t.IsLeaf.GetValueOrDefault());
        }

        public List<Data.Entities.ObjectAssignedUser> GetAllIncompleteByDoc(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.ActionTypeId != 5
                                               ).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllIncompleteByDoc()
        {
            return this.repo.GetAll().Where(t => !t.IsComplete.GetValueOrDefault()
                                                && t.IsLeaf.GetValueOrDefault()
                                                && t.ActionTypeId != 5
                                               ).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllListByDoc(List<Guid> ListobjId)
        {
            return this.repo.GetAll().Where(t => ListobjId.Contains(t.ObjectID.GetValueOrDefault())
                                               ).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllListObjWF(int wfId,Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID==objId && t.WorkflowId==wfId).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllListObjID(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllListObjAssignWF(Guid objId)
        {
            return this.repo.GetAll().Where(t =>t.ObjectAssignedWorkflowID.GetValueOrDefault()==objId ).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetIncompletetObjAssignWF(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectAssignedWorkflowID.GetValueOrDefault() == objId && t.IsComplete.GetValueOrDefault()).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllManagementIncompleteByDoc(Guid objId, int wfId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.ActionTypeId == 5
                                                && t.WorkflowId == wfId
                                               ).ToList();
        }

        public List<Data.Entities.ObjectAssignedUser> GetAllIncompleteManagementByDoc(Guid objId, string objType)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.ActionTypeId == 2
                                                && t.ObjectType == objType).ToList();
        }

        public List<Data.Entities.ObjectAssignedUser> GetAllOverDueTask(string objType)
        {
            return this.repo.GetAll().Where(t => t.ObjectType == objType
                                                && !t.IsComplete.GetValueOrDefault()
                                                && t.PlanCompleteDate != null
                                                && t.PlanCompleteDate < DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null)).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllOverDueTask()
        {
            return this.repo.GetAll().Where(t => !t.IsComplete.GetValueOrDefault()
                                                && t.PlanCompleteDate != null
                                                && t.PlanCompleteDate.GetValueOrDefault().Date < DateTime.Now.Date).ToList();
        }
        public List<Data.Entities.ObjectAssignedUser> GetAllByObj(Guid objId, string objType)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId
                                                && t.ObjectType == objType).ToList();
        }

        //public List<Data.Entities.ObjectAssignedUser> GetAllWorkingHistoryByObj(Guid objId)
        //{
        //    return this.repo.GetAll().Where(t => t.ObjectID == objId
        //                                        && t.ActionTypeId != 5).ToList();
        //}
        public List<Data.Entities.ObjectAssignedUser> GetAllWorkingHistoryByObj(Guid objId)
        {
            return this.repo.GetAll().Where(t => t.ObjectID == objId).ToList();
        }
        public Data.Entities.ObjectAssignedUser GetCurrentInCompleteByDocUser(int userId, Guid objId, string objType)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ObjectID == objId 
                                                        && t.UserID == userId 
                                                        && !t.IsComplete.GetValueOrDefault()
                                                        && t.ObjectType == objType);
        }
        public Data.Entities.ObjectAssignedUser GetCurrentInCompleteByDocUseraction(int userId, Guid objId, int objType, int Action)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ObjectID == objId
                                                        && t.UserID == userId
                                                        && !t.IsComplete.GetValueOrDefault()
                                                        && t.ObjectTypeId == objType
                                                        && t.ActionTypeId==Action);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Data.Entities.ObjectAssignedUser> GetAll()
        {
            return this.repo.GetAll().ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Data.Entities.ObjectAssignedUser GetById(Guid id)
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
        public Guid? Insert(Data.Entities.ObjectAssignedUser bo)
        {
            var objId = this.repo.Insert(bo);
            // Trigger data change
            if (objId != null)
            {
                var changeData = new WaitingSyncData()
                {
                    ActionTypeID = 1,
                    ActionTypeName = "Insert",
                    ObjectID = objId,
                    ObjectName = "[WMS].[ObjectAssignedUser]",
                    EffectDate = DateTime.Now,IsSynced = false
                };

                this.waitingSyncDataService.Insert(changeData);
            }

            // -------------------------------------------------------

            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Data.Entities.ObjectAssignedUser bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                if (flag)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 2,
                        ActionTypeName = "Update",
                        ObjectID = bo.ID,
                        ObjectName = "[WMS].[ObjectAssignedUser]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

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
        public bool Delete(Data.Entities.ObjectAssignedUser bo)
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
        public bool Delete(Guid id)
        {
            try
            {
                var flag = this.repo.Delete(id);
                if (flag)
                {
                    // Trigger data change
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 3,
                        ActionTypeName = "Delete",
                        ObjectID = id,
                        ObjectName = "[WMS].[ObjectAssignedUser]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                    // ----------------------------------------------------------------------
                }

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
