using EDMs.Business.Services.Security;

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
    public class MaterialService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly MaterialDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialCodeService"/> class.
        /// </summary>
        public MaterialService()
        {
            this.repo = new MaterialDAO();
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
        public List<MaterialCode> GetAll()
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
        public MaterialCode GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public MaterialCode GetByCode(string code)
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
        public int? Insert(MaterialCode bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(MaterialCode bo)
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
        public bool Delete(MaterialCode bo)
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
