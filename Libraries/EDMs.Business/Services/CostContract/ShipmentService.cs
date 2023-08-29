using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.CostContract
{
   public class ShipmentService
    {/// <summary>
     /// The repo.
     /// </summary>
        private readonly ShipmentDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentService"/> class.
        /// </summary>
        public ShipmentService()
        {
            this.repo = new ShipmentDAO();
        }

        #region Get (Advances)

        public List<Shipment> GetAllByProject(int projectID)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectID).OrderBy(t => t.Number).ToList();
        }

        public List<Shipment> GetAllByProjectAndType(int projectID, string typeid)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectID && t.ShipmentTypeId == typeid).OrderBy(t => t.Number).ToList();
        }

        public Shipment GetByNumber(string number)
        {
            return this.repo.GetAll().Find(t => t.Number == number);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Shipment> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.Number).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Shipment GetById(Guid id)
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
        public bool Insert(Shipment bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Shipment bo)
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
        public bool Delete(Shipment bo)
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
        public bool Delete(Guid id)
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
