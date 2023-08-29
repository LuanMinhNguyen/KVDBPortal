using System;

namespace EDMs.Business.Services.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;

    public class PermissionService
    {      
        private readonly PermissionDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        public PermissionService()
        {
            this.repo = new PermissionDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<Permission> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Permission GetByID(int id)
        {
            return this.repo.GetById(id);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public List<Permission> GetByRoleId(int roleId)
        {
            return this.repo.GetByRoleId(roleId);
        }

        public List<Permission> GetByRoleId(int roleId, int specialParent)
        {
            return this.repo.GetByRoleId(roleId, specialParent).Where(t => t.Menu.Active == true).ToList();
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        public bool DeletePermissions(List<Permission> permissions)
        {
            try
            {
                foreach (var permission in permissions)
                {
                    var flag = this.repo.Delete(permission.Id);
                    if (flag)
                    {
                        // Trigger data change
                        var changeData = new WaitingSyncData()
                        {
                            ActionTypeID = 3,
                            ActionTypeName = "Delete",
                            ObjectID2 = permission.Id,
                            ObjectName = "[Security].[Permissions]",
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
        /// Adds the permissions.
        /// </summary>
        /// <param name="permissions">The permissions.</param>
        /// <returns></returns>
        public bool AddPermissions(List<Permission> permissions)
        {
            try
            {
                foreach (var permission in permissions)
                {
                    var objId = this.repo.Insert(permission);
                    // Trigger data change
                    if (objId != null)
                    {
                        var changeData = new WaitingSyncData()
                        {
                            ActionTypeID = 1,
                            ActionTypeName = "Insert",
                            ObjectID2 = objId,
                            ObjectName = "[Security].[Permissions]",
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
