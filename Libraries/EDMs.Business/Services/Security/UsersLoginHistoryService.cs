

namespace EDMs.Business.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;
    public  class UsersLoginHistoryService
    {
        private readonly UsersLoginHistoryDAO repo;

        public UsersLoginHistoryService()
        {
            this.repo = new  UsersLoginHistoryDAO();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All User
        /// </summary>
        /// <returns></returns>
        public List<UsersLoginHistory> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get User By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public UsersLoginHistory GetByID(int ID)
        {
            return this.repo.GetByID(ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public List<UsersLoginHistory> GetAllList(List<int> ListID)
        {
            return this.repo.GetAll().Where(t => ListID.Contains(t.ID)).ToList();
        }

        public UsersLoginHistory GetUserByUsername(string username, bool ison)
        {
            return this.repo.GetUsersLoginHistoryByname(username,ison);
        }

        public List<UsersLoginHistory> GetAllListByDate(DateTime fromdate, DateTime todate)
        {

           return this.repo.GetAll().Where(ob => ob.ServerTime.GetValueOrDefault().Date >= fromdate.Date && ob.ServerTime.GetValueOrDefault().Date <= todate.Date).ToList();
        }

     
        #endregion

        #region Insert, Update, Delete
        

        /// <summary>
        /// Insert User
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Insert(UsersLoginHistory bo)
        {
            try
            {
                var flag = this.repo.Insert(bo);

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
        public bool Update(UsersLoginHistory bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
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
        public bool Delete(UsersLoginHistory bo)
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
