namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Library;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class UnitService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly UnitDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitService"/> class.
        /// </summary>
        public UnitService()
        {
            this.repo = new UnitDAO();
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
        public List<Unit> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get All Unit Active
        /// </summary>
        /// <returns></returns>
        public List<Unit> GetAllActive()
        {
            return this.repo.GetAll().Where(t=> t.Active.GetValueOrDefault()).ToList();
        }
        /// <summary>
        /// Get All Unit Of Unit
        /// </summary>
        /// <returns></returns>
        public List<Unit> GetAllArea(int AreaId)
        {
            return this.repo.GetAll().Where(t => t.AreaId.GetValueOrDefault() == AreaId).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Unit GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public Unit GetByName(string name)
        {
            return this.repo.GetByName(name);
        }

        public Unit GetByCode(string code)
        {
            return this.repo.GetByCode(code);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(Unit bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Unit bo)
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
        public bool Delete(Unit bo)
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
