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
    public class BlockService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly BlockDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockService"/> class.
        /// </summary>
        public BlockService()
        {
            this.repo = new BlockDAO();
        }

        #region Get (Advances)
        ////public List<Block> GetAllWithoutDeparment()
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == 0 || t.RoleId == null).ToList();
        ////}

        ////public List<Block> GetAllByDeparment(int deparmentId)
        ////{
        ////    return this.repo.GetAll().Where(t => t.RoleId == deparmentId).ToList();
        ////}
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Block> GetAll()
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
        public Block GetById(int id)
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
        public int? Insert(Block bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Block bo)
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
        public bool Delete(Block bo)
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
