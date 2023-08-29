﻿using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Scope;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Scope
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class ContractorService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ContractorDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractorService"/> class.
        /// </summary>
        public ContractorService()
        {
            this.repo = new ContractorDAO();
        }

        #region Get (Advances)

        public List<Contractor> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectId).ToList();
        }


        public Contractor GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim());
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Contractor> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.Name).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Contractor GetById(int id)
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
        public int? Insert(Contractor bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Contractor bo)
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
        public bool Delete(Contractor bo)
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
