namespace EDMs.Business.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;

    public class UserService
    {      
        private readonly UserDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        public UserService()
        {
            this.repo = new UserDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        public List<User> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get User By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public User GetByID(int ID)
        {
            return this.repo.GetByID( ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public List<User>  GetAllByRoleId(int roleId)
        {
            return this.repo.GetAllByRoleId(roleId);
        }

        public List<User> GetAllByTitle(int titleId)
        {
            return this.repo.GetAll().Where(t => t.TitleId == titleId).ToList();
        }
        public List<User> GetAllByDC()
        {
            return this.repo.GetAll().Where(t => t.IsDC.GetValueOrDefault()).ToList();
        }
        public User GetUserByUsername(string username)
        {
            return this.repo.GetUserByUsername(username);
        }

        public List<User> GetSpecialListUser(List<int> roleIds)
        {
            return this.repo.GetSpecialListUser(roleIds);
        }
        public List<User> GetListUser(List<int> ListIds)
        {
            return this.repo.GetAll().Where(t=> ListIds.Contains(t.Id)).ToList();
        }
        public User GetByResourceId(int resourceId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.ResourceId == resourceId);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public bool ChangePassword(int userId, string newPassword)
        {
            return this.repo.ChangePassword(userId, newPassword);
        }

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Insert(User bo)
        {
            try
            {
                var flag = this.repo.Insert(bo);
                // Trigger data change
                if (flag)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 1,
                        ActionTypeName = "Insert",
                        ObjectID2 = bo.Id,
                        ObjectName = "[Security].[Users]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

                // -------------------------------------------------------

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update User
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(User bo)
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
                        ObjectID2 = bo.Id,
                        ObjectName = "[Security].[Users]",
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
        /// Delete User
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(User bo)
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
        /// Delete User By ID
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
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 3,
                        ActionTypeName = "Delete",
                        ObjectID2 = id,
                        ObjectName = "[Security].[Users]",
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

        /// <summary>
        /// Checks the exists.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool CheckExists(int? userId, string userName)
        {
            return this.repo.CheckExists(userId, userName);
        }
    }
}
