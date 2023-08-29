using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.DAO.Security;
using EAM.Data.Entities;

namespace EAM.Business.Services.Security
{
    public class AA_UsersService
    {      
        private readonly AA_UsersDAO repo;


        public AA_UsersService()
        {
            this.repo = new AA_UsersDAO();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All AA_Users
        /// </summary>
        /// <returns></returns>
        public List<AA_Users> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get AA_Users By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AA_Users GetByID(int ID)
        {
            return this.repo.GetByID( ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public List<AA_Users>  GetAllByRoleId(int roleId)
        {
            return this.repo.GetAllByRoleId(roleId);
        }

        public List<AA_Users> GetAllByTitle(int titleId)
        {
            return this.repo.GetAll().Where(t => t.TitleId == titleId).ToList();
        }
        public List<AA_Users> GetAllByDC()
        {
            return this.repo.GetAll().Where(t => t.IsDC.GetValueOrDefault()).ToList();
        }
        public AA_Users GetByUsername(string AA_Usersname)
        {
            return this.repo.GetAA_UsersByAA_Usersname(AA_Usersname);
        }

        public List<AA_Users> GetSpecialListAA_Users(List<int> roleIds)
        {
            return this.repo.GetSpecialListAA_Users(roleIds);
        }
        public List<AA_Users> GetListAA_Users(List<int> ListIds)
        {
            return this.repo.GetAll().Where(t=> ListIds.Contains(t.Id)).ToList();
        }
        public AA_Users GetByResourceId(int resourceId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ResourceId == resourceId);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="AA_UsersId">The AA_Users id.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public bool ChangePassword(int AA_UsersId, string newPassword)
        {
            return this.repo.ChangePassword(AA_UsersId, newPassword);
        }

        /// <summary>
        /// Insert AA_Users
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Insert(AA_Users bo)
        {
            try
            {
                var flag = this.repo.Insert(bo);
                // Trigger data change
                ////if (flag)
                ////{
                ////    var changeData = new WaitingSyncData()
                ////    {
                ////        ActionTypeID = 1,
                ////        ActionTypeName = "Insert",
                ////        ObjectID2 = bo.Id,
                ////        ObjectName = "[Security].[AA_Userss]",
                ////        EffectDate = DateTime.Now,IsSynced = false
                ////    };

                ////    this.waitingSyncDataService.Insert(changeData);
                ////}

                // -------------------------------------------------------

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update AA_Users
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(AA_Users bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                ////if (flag)
                ////{
                ////    var changeData = new WaitingSyncData()
                ////    {
                ////        ActionTypeID = 2,
                ////        ActionTypeName = "Update",
                ////        ObjectID2 = bo.Id,
                ////        ObjectName = "[Security].[AA_Userss]",
                ////        EffectDate = DateTime.Now,IsSynced = false
                ////    };

                ////    this.waitingSyncDataService.Insert(changeData);
                ////}

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete AA_Users
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(AA_Users bo)
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
        /// Delete AA_Users By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var flag = this.repo.Delete(id);
                if (flag)
                {
                    // Trigger data change
                    ////var changeData = new WaitingSyncData()
                    ////{
                    ////    ActionTypeID = 3,
                    ////    ActionTypeName = "Delete",
                    ////    ObjectID2 = id,
                    ////    ObjectName = "[Security].[AA_Userss]",
                    ////    EffectDate = DateTime.Now,IsSynced = false
                    ////};

                    ////this.waitingSyncDataService.Insert(changeData);
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

        /// <summary>
        /// Checks the exists.
        /// </summary>
        /// <param name="AA_UsersId">The AA_Users id.</param>
        /// <param name="AA_UsersName">Name of the AA_Users.</param>
        /// <returns></returns>
        public bool CheckExists(int? AA_UsersId, string AA_UsersName)
        {
            return this.repo.CheckExists(AA_UsersId, AA_UsersName);
        }
    }
}
