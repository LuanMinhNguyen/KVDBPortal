using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Security;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Security
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class UserDisciplineService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly UserDisciplineDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserDisciplineService"/> class.
        /// </summary>
        public UserDisciplineService()
        {
            this.repo = new UserDisciplineDAO();
        }

        #region Get (Advances)
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<UserDiscipline> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get All UserDiscipline Active
        /// </summary>
        /// <returns></returns>
        public List<UserDiscipline> GetAllActive()
        {
            return this.repo.GetAll().Where(t=> t.Active.GetValueOrDefault()).ToList();
        }
        
        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public UserDiscipline GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public UserDiscipline GetByName(string _Name)
        {
            return this.repo.GetByName(_Name);
        }

        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(UserDiscipline bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(UserDiscipline bo)
        {
            try
            {
                return this.repo.Update(bo);
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
        public bool Delete(UserDiscipline bo)
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
                return this.repo.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
