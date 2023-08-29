

namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Library;
    using EDMs.Data.Entities;
    public  class CustomizeReceivedEmailService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly CustomizeReceivedEmailDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizeReceivedEmailService"/> class.
        /// </summary>
        public CustomizeReceivedEmailService()
        {
            this.repo = new CustomizeReceivedEmailDAO();
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
        public List<CustomizeReceivedEmail> GetAll()
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
        public CustomizeReceivedEmail GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public CustomizeReceivedEmail GetByName(string _Name)
        {
            return this.repo.GetByName(_Name);
        }

        public CustomizeReceivedEmail GetByType(int typeid, int from)
        {
            return this.repo.GetByType(typeid,from);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(CustomizeReceivedEmail bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(CustomizeReceivedEmail bo)
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
        public bool Delete(CustomizeReceivedEmail bo)
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
