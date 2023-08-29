using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;
namespace EDMs.Business.Services.CostContract
{
  public  class ShipmentDetailService
    {/// <summary>
     /// The repo.
     /// </summary>
        private readonly ShipmentDetailDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentDetailService"/> class.
        /// </summary>
        public ShipmentDetailService()
        {
            this.repo = new ShipmentDetailDAO();
        }

        #region Get (Advances)

        public List<ShipmentDetail> GetByShipmentNumber(string number)
        {
            return this.repo.GetAll().Where(t => t.ShipmentNumber == number).ToList();
        }

        public List<ShipmentDetail> GetByShipmentId(Guid Id)
        {
            return this.repo.GetAllShipmentId(Id);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ShipmentDetail> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.ID).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public ShipmentDetail GetById(int id)
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
        public int? Insert(ShipmentDetail bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ShipmentDetail bo)
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
        public bool Delete(ShipmentDetail bo)
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
