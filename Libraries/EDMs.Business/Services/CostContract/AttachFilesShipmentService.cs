using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Document;
using EDMs.Data.DAO.CostContract;
using EDMs.Data.Entities;


namespace EDMs.Business.Services.CostContract
{
  public  class AttachFilesShipmentService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly AttachFilesShipmentDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="AttachFilesShipmentService"/> class.
        /// </summary>
        public AttachFilesShipmentService()
        {
            this.repo = new AttachFilesShipmentDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get specific.
        /// </summary>
        /// <param name="tranId">
        /// The tran id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AttachFilesShipment> GetSpecific(int tranId)
        {
            return this.repo.GetSpecific(tranId);
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<AttachFilesShipment> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        /// <summary>
        /// The get all by doc id.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AttachFilesShipment> GetAllByShipment(Guid WorkpackageId)
        {
            return this.repo.GetAll().Where(t => t.ShipmentId == WorkpackageId).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public AttachFilesShipment GetById(int id)
        {
            return this.repo.GetById(id);
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="AttachFilesShipment"/>.
        /// </returns>
        public AttachFilesShipment GetByNameServer(string nameServer)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.FilePath.ToLower().Contains(nameServer.ToLower()));
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(AttachFilesShipment bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(AttachFilesShipment bo)
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

        public bool Update(AttachFilesShipment src)
        {
            return this.repo.Update(src);
        }
        #endregion
    }
}
