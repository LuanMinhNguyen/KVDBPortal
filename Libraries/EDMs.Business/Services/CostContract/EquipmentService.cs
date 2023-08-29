using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Library;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.CostContract
{
 public  class EquipmentService
    { /// <summary>
      /// The repo.
      /// </summary>
        private readonly EquipmentDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentService"/> class.
        /// </summary>
        public EquipmentService()
        {
            this.repo = new EquipmentDAO();
        }

        #region Get (Advances)

        public List<Equipment> GetAllByProject(int projectID)
        {
            return this.repo.GetAll().Where(t => t.ProjectID == projectID).OrderBy(t => t.Number).ToList();
        }

        public Equipment GetByNumber(string number)
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
        public List<Equipment> GetAll()
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
        public Equipment GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public List<Equipment> GetByIdparrent(int Id)
        {
            return this.repo.GetAllParrentId(Id).OrderBy(t => t.Number).ToList();
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(Equipment bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Equipment bo)
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
        public bool Delete(Equipment bo)
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
