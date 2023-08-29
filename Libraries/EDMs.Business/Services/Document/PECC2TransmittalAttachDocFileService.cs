// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PECC2TransmittalAttachDocFileService.cs" company="">
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
    public class PECC2TransmittalAttachDocFileService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PECC2TransmittalAttachDocFileDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2TransmittalAttachDocFileService"/> class.
        /// </summary>
        public PECC2TransmittalAttachDocFileService()
        {
            this.repo = new PECC2TransmittalAttachDocFileDAO();
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
        public List<PECC2TransmittalAttachDocFiles> GetByTrans(Guid tranId)
        {
            return this.repo.GetAll().Where(t => t.TransId == tranId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PECC2TransmittalAttachDocFiles> GetAll()
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
        public PECC2TransmittalAttachDocFiles GetById(Guid id)
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
        /// The <see cref="PECC2TransmittalAttachDocFile"/>.
        /// </returns>
        public PECC2TransmittalAttachDocFiles GetByNameServer(string nameServer)
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
        public Guid? Insert(PECC2TransmittalAttachDocFiles bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(PECC2TransmittalAttachDocFiles bo)
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
