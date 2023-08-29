// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ContractorTransmittalDocFileService.cs" company="">
//   
// </copyright>
// <summary>
//   The category service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class ContractorTransmittalDocFileService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly ContractorTransmittalDocFileDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractorTransmittalDocFileService"/> class.
        /// </summary>
        public ContractorTransmittalDocFileService()
        {
            this.repo = new ContractorTransmittalDocFileDAO();
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
        public List<ContractorTransmittalDocFile> GetSpecific(Guid tranId)
        {
            return this.repo.GetSpecific(tranId);
        }

        public List<ContractorTransmittalDocFile> GetAllByTrans(Guid transId)
        {
            return this.repo.GetAll().Where(t => t.TransId == transId).ToList();
        }

        public List<ContractorTransmittalDocFile> GetAllByTrans(Guid transId, int typeId)
        {
            return this.repo.GetAll().Where(t => t.TransId == transId && t.TypeId == typeId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<ContractorTransmittalDocFile> GetAll()
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
        public ContractorTransmittalDocFile GetById(Guid id)
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
        /// The <see cref="ContractorTransmittalDocFile"/>.
        /// </returns>
        public ContractorTransmittalDocFile GetByNameServer(string nameServer)
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
        public Guid? Insert(ContractorTransmittalDocFile bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ContractorTransmittalDocFile bo)
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
        public bool Delete(ContractorTransmittalDocFile bo)
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
