namespace EDMs.Business.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Security;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class OptionalTypePropertiesViewService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly OptionalTypePropertiesViewDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalTypePropertiesViewService"/> class.
        /// </summary>
        public OptionalTypePropertiesViewService()
        {
            this.repo = new OptionalTypePropertiesViewDAO();
        }

        #region Get (Advances)
        ////public List<OptionalTypePropertiesView> GetAllWithoutDeparment()
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == 0 || t.RoleId == null).ToList();
        ////}

        ////public List<OptionalTypePropertiesView> GetAllByDeparment(int deparmentId)
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == deparmentId).ToList();
        ////}
        #endregion

        public OptionalTypePropertiesView GetByOptionalType(int optionalTypeId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.OptionalTypeId == optionalTypeId);
        }

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<OptionalTypePropertiesView> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<OptionalTypePropertiesView> GetAllSpecial(int optionalType)
        {
            return
                this.repo.GetAll().Where(
                    t =>
                    t.OptionalTypeId == optionalType).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public OptionalTypePropertiesView GetById(int id)
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
        public int? Insert(OptionalTypePropertiesView bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(OptionalTypePropertiesView bo)
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
        public bool Delete(OptionalTypePropertiesView bo)
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
