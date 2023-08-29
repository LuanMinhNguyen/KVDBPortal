﻿namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data.DAO.Library;
    using Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class SystemCodeService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly SystemCodeDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemCodeService"/> class.
        /// </summary>
        public SystemCodeService()
        {
            this.repo = new SystemCodeDAO();
        }

        #region Get (Advances)
        public SystemCode GetByCode(string code)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Code == code);
        }

        public List<SystemCode> GetAllSubSystem(int systemId)
        {
            return this.repo.GetAll().Where(t => t.ParentId == systemId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<SystemCode> GetAll()
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
        public SystemCode GetById(int id)
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
        public int? Insert(SystemCode bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(SystemCode bo)
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
        public bool Delete(SystemCode bo)
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
