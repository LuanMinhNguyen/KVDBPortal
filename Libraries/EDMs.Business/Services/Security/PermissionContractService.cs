using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.DAO.Security;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.Security
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class PermissionContractService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PermissionContractDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionContractService"/> class.
        /// </summary>
        public PermissionContractService()
        {
            this.repo = new PermissionContractDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get user id list in permission.
        /// </summary>
        /// <param name="packageId">
        /// The package id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> GetUserIdListInPermission(int contractID)
        {
            return this.repo.GetAll().Where(t => t.ContractID == contractID).Select(t => t.UserId.GetValueOrDefault()).ToList();
        }

        /// <summary>
        /// The get all by user.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PermissionContract> GetAllByUser(int userId)
        {
            return this.repo.GetAll().Where(t => t.UserId == userId).ToList();
        }

        public List<PermissionContract> GetAllByContract(int contractId)
        {
            return this.repo.GetAll().Where(t => t.ContractID == contractId).ToList();
        }
        public bool IsFullPermission(int userId, int contracID)
        {
            var permissionObj =
                this.repo.GetAll().FirstOrDefault(t => t.UserId == userId && t.ContractID == contracID);
            return permissionObj != null && permissionObj.IsFullPermission.GetValueOrDefault();
        }
        

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PermissionContract> GetAll()
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
        public PermissionContract GetById(int id)
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
        public int? Insert(PermissionContract bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(PermissionContract bo)
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
