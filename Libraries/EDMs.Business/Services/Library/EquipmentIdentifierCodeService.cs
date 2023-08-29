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
    public class EquipmentIdentifierCodeService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly EquipmentIdentifierCodeDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentIdentifierCodeService"/> class.
        /// </summary>
        public EquipmentIdentifierCodeService()
        {
            this.repo = new EquipmentIdentifierCodeDAO();
        }

        #region Get (Advances)

        public List<EquipmentIdentifierCode> GetAllWithoutDeparment()
        {
            return this.repo.GetAll();//.Where(t => t.Block != null && (t.Block.RoleId == 0 || t.Block.RoleId == null)).ToList();
        }

        public List<EquipmentIdentifierCode> GetAllByDeparment(int deparmentId)
        {
            return this.repo.GetAll();//.Where(t => t.Block != null && t.Block.RoleId == deparmentId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<EquipmentIdentifierCode> GetAll()
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
        public EquipmentIdentifierCode GetById(int id)
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
        public int? Insert(EquipmentIdentifierCode bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(EquipmentIdentifierCode bo)
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
        public bool Delete(EquipmentIdentifierCode bo)
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
