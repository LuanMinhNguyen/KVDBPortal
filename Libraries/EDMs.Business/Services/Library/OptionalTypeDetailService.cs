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
    public class OptionalTypeDetailService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly OptionalTypeDetailDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionalTypeDetailService"/> class.
        /// </summary>
        public OptionalTypeDetailService()
        {
            this.repo = new OptionalTypeDetailDAO();
        }

        #region Get (Advances)
        ////public List<OptionalTypeDetail> GetAllWithoutDeparment()
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == 0 || t.RoleId == null).ToList();
        ////}

        ////public List<OptionalTypeDetail> GetAllByDeparment(int deparmentId)
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == deparmentId).ToList();
        ////}
        
        public List<int> GetAllChildId(int optionalTypeId, List<int> filterList)
        {
            return this.repo.GetAll().Where(t => t.OptionalTypeId == optionalTypeId && filterList.Contains(t.ID)).Select(t => t.ID).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<OptionalTypeDetail> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<OptionalTypeDetail> GetAllSpecial(string categoryId, int optionalTypeId, string roleId)
        {
            return this.repo.GetAll().Where(t =>
                (categoryId == "0" || (t.CategoryId.Contains(categoryId) || t.CategoryId == "0" || string.IsNullOrEmpty(t.CategoryId))) &&
                (optionalTypeId == 0 || t.OptionalTypeId == optionalTypeId) &&
                (roleId == "0" || t.RoleId.Contains(roleId) || string.IsNullOrEmpty(t.RoleId))).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public OptionalTypeDetail GetById(int id)
        {
            return this.repo.GetById(id);
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="optionalTypeId">
        /// The optional type id.
        /// </param>
        /// <returns>
        /// The <see cref="OptionalTypeDetail"/>.
        /// </returns>
        public OptionalTypeDetail GetByName(string name, int optionalTypeId)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.OptionalTypeId == optionalTypeId && t.Name == name);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(OptionalTypeDetail bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(OptionalTypeDetail bo)
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
        public bool Delete(OptionalTypeDetail bo)
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
