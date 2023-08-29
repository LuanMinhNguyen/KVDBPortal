using System;

namespace EDMs.Business.Services.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;

    public class SharePermissionService
    {      
        private readonly SharePermissionDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        public SharePermissionService()
        {
            this.repo = new SharePermissionDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<SharePermission> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public SharePermission GetByID(Guid id)
        {
            return this.repo.GetById(id);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public List<SharePermission> GetByFromUser(int userId)
        {
            return this.repo.GetByFromUser(userId);
        }

        public List<SharePermission> GetByFromUserAndObj(int userId, string objId)
        {
            return this.repo.GetAll().Where(t => t.FromUserId == userId && t.ObjectList.Split(';').Contains(objId)).ToList();
        }

        public List<SharePermission> GetByToUser(int userId)
        {
            return this.repo.GetAll().Where(t => t.ToUserId == userId).ToList();
        }

        public SharePermission GetByFromToUser(int fromUserId, int toUserId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ToUserId == toUserId && t.FromUserId == fromUserId);
        }

        #endregion

        #region Insert, Update, Delete

        public Guid? Insert(SharePermission bo)
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
                    ObjectName = "[Security].[SharePermissions]",
                    EffectDate = DateTime.Now,
                    IsSynced = false
                };

                this.waitingSyncDataService.Insert(changeData);
            }

            // -------------------------------------------------------

            return objId;
        }

        public bool Update(SharePermission bo)
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
                        ObjectName = "[WMS].[SharePermissions]",
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

        /// <summary>
        /// Deletes the SharePermissions.
        /// </summary>
        /// <param name="SharePermissions">The SharePermissions.</param>
        /// <returns></returns>
        public bool DeleteSharePermissions(List<SharePermission> SharePermissions)
        {
            try
            {
                foreach (var SharePermission in SharePermissions)
                {
                    var flag = this.repo.Delete(SharePermission.ID);
                    if (flag)
                    {
                        // Trigger data change
                        var changeData = new WaitingSyncData()
                        {
                            ActionTypeID = 3,
                            ActionTypeName = "Delete",
                            ObjectID = SharePermission.ID,
                            ObjectName = "[Security].[SharePermissions]",
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
        /// Adds the SharePermissions.
        /// </summary>
        /// <param name="SharePermissions">The SharePermissions.</param>
        /// <returns></returns>
        public bool AddSharePermissions(List<SharePermission> SharePermissions)
        {
            try
            {
                foreach (var SharePermission in SharePermissions)
                {
                    var objId = this.repo.Insert(SharePermission);
                    // Trigger data change
                    if (objId != null)
                    {
                        var changeData = new WaitingSyncData()
                        {
                            ActionTypeID = 1,
                            ActionTypeName = "Insert",
                            ObjectID = objId,
                            ObjectName = "[Security].[SharePermissions]",
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

        #endregion
        
    }
}
