using System;

namespace EDMs.Business.Services.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;

    public class FunctionPermissionService
    {      
        private readonly FunctionPermissionDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        public FunctionPermissionService()
        {
            this.repo = new FunctionPermissionDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<FunctionPermission> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public FunctionPermission GetByID(int id)
        {
            return this.repo.GetById(id);
        }
        #endregion

        #region Get (Advances)
        public List<FunctionPermission> GetAllByUser(int userId)
        {
            return this.repo.GetAll().Where(t => t.UserId == userId).ToList();
        }

        public FunctionPermission GetOne(int userId, int objTypeId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.UserId == userId && t.ObjectTypeId == objTypeId);
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the FunctionPermissions.
        /// </summary>
        /// <param name="FunctionPermissions">The FunctionPermissions.</param>
        /// <returns></returns>
        public bool Delete(List<FunctionPermission> FunctionPermissions)
        {
            try
            {
                foreach (var FunctionPermission in FunctionPermissions)
                {
                    var flag = this.repo.Delete(FunctionPermission.ID);
                    if (flag)
                    {
                        // Trigger data change
                        var changeData = new WaitingSyncData()
                        {
                            ActionTypeID = 3,
                            ActionTypeName = "Delete",
                            ObjectID2 = FunctionPermission.ID,
                            ObjectName = "[Security].[FunctionPermission]",
                            EffectDate = DateTime.Now,IsSynced = false
                        };

                        this.waitingSyncDataService.Insert(changeData);
                        // ----------------------------------------------------------------------
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds the FunctionPermissions.
        /// </summary>
        /// <param name="FunctionPermissions">The FunctionPermissions.</param>
        /// <returns></returns>
        public bool AddFunctionPermissions(List<FunctionPermission> FunctionPermissions)
        {
            try
            {
                foreach (var FunctionPermission in FunctionPermissions)
                {
                    var objId = this.repo.Insert(FunctionPermission);
                    // Trigger data change
                    if (objId != null)
                    {
                        var changeData = new WaitingSyncData()
                        {
                            ActionTypeID = 1,
                            ActionTypeName = "Insert",
                            ObjectID2 = objId,
                            ObjectName = "[Security].[FunctionPermission]",
                            EffectDate = DateTime.Now,IsSynced = false
                        };

                        this.waitingSyncDataService.Insert(changeData);
                    }

                    // -------------------------------------------------------
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public int? Insert(FunctionPermission bo)
        {
            try
            {
                var flag = this.repo.Insert(bo);
                // Trigger data change
                if (flag != null)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 1,
                        ActionTypeName = "Insert",
                        ObjectID2 = bo.ID,
                        ObjectName = "[Security].[FunctionPermission]",
                        EffectDate = DateTime.Now,
                        IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

                // -------------------------------------------------------

                return flag;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool Update(FunctionPermission bo)
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
                        ObjectID2 = bo.ID,
                        ObjectName = "[Security].[FunctionPermission]",
                        EffectDate = DateTime.Now,
                        IsSynced = false
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

        #endregion

    }
}
