using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.DAO.Security;
using EAM.Data.Entities;

namespace EAM.Business.Services.Security
{
    public class AA_PermissionsService
    {      
        private readonly AA_PermissionsDAO repo;

        //private readonly WaitingSyncDataService waitingSyncDataService;

        public AA_PermissionsService()
        {
            this.repo = new AA_PermissionsDAO();
            //this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<AA_Permissions> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AA_Permissions GetByID(int id)
        {
            return this.repo.GetById(id);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public List<AA_Permissions> GetByRoleId(int roleId)
        {
            return this.repo.GetByRoleId(roleId);
        }

        public List<AA_Permissions> GetByRoleId(int roleId, int specialParent)
        {
            return this.repo.GetByRoleId(roleId, specialParent).Where(t => t.Menu.Active == true).ToList();
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the AA_Permissionss.
        /// </summary>
        /// <param name="AA_Permissionss">The AA_Permissionss.</param>
        /// <returns></returns>
        public bool DeletePermissionss(List<AA_Permissions> AA_Permissionss)
        {
            try
            {
                foreach (var AA_Permissions in AA_Permissionss)
                {
                    var flag = this.repo.Delete(AA_Permissions.Id);
                    //if (flag)
                    //{
                    //    // Trigger data change
                    //    var changeData = new WaitingSyncData()
                    //    {
                    //        ActionTypeID = 3,
                    //        ActionTypeName = "Delete",
                    //        ObjectID2 = AA_Permissions.Id,
                    //        ObjectName = "[Security].[AA_Permissionss]",
                    //        EffectDate = DateTime.Now,IsSynced = false
                    //    };

                    //    this.waitingSyncDataService.Insert(changeData);
                    //    // ----------------------------------------------------------------------
                    //}
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Adds the AA_Permissionss.
        /// </summary>
        /// <param name="AA_Permissionss">The AA_Permissionss.</param>
        /// <returns></returns>
        public bool AddPermissionss(List<AA_Permissions> AA_Permissionss)
        {
            try
            {
                foreach (var AA_Permissions in AA_Permissionss)
                {
                    var objId = this.repo.Insert(AA_Permissions);
                    // Trigger data change
                    ////if (objId != null)
                    ////{
                    ////    var changeData = new WaitingSyncData()
                    ////    {
                    ////        ActionTypeID = 1,
                    ////        ActionTypeName = "Insert",
                    ////        ObjectID2 = objId,
                    ////        ObjectName = "[Security].[AA_Permissionss]",
                    ////        EffectDate = DateTime.Now,IsSynced = false
                    ////    };

                    ////    this.waitingSyncDataService.Insert(changeData);
                    ////}

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
