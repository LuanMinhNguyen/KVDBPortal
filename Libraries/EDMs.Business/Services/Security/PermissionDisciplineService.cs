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
    public class PermissionDisciplineService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PermissionDisciplineDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionDisciplineService"/> class.
        /// </summary>
        public PermissionDisciplineService()
        {
            this.repo = new PermissionDisciplineDAO();
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
        public List<int> GetUserIdListInPermission(int disciplineID)
        {
            return this.repo.GetAll().Where(t => t.DisciplineID == disciplineID).Select(t => t.UserId.GetValueOrDefault()).ToList();
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
        public List<PermissionDiscipline> GetAllByUser(int userId)
        {
            return this.repo.GetAll().Where(t => t.UserId == userId).ToList();
        }

        public List<PermissionDiscipline> GetAllByDiscipline(int disciplineId)
        {
            return this.repo.GetAll().Where(t => t.DisciplineID == disciplineId).ToList();
        }

        public bool IsFullPermission(int userId, int disciplineId)
        {
            var permissionObj =
                this.repo.GetAll().FirstOrDefault(t => t.UserId == userId && t.DisciplineID == disciplineId);
            return permissionObj != null && permissionObj.IsFullPermission.GetValueOrDefault();
        }
        /// <summary>
        /// The get project in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> GetProjectInPermission(int userId)
        {
            return this.repo.GetAll().Where(t => t.UserId == userId).Select(t => t.ProjectId.GetValueOrDefault()).Distinct().ToList();
        }

        /// <summary>
        /// The get package in permission.
        /// </summary>
        /// <param name="userId">
        /// The user id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<int> GetDisciplineInPermission(int userId)
        {
            return
                this.repo.GetAll().Where(t => t.UserId == userId).Select(t => t.DisciplineID.GetValueOrDefault()).ToList();
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PermissionDiscipline> GetAll()
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
        public PermissionDiscipline GetById(int id)
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
        public int? Insert(PermissionDiscipline bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Delete Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(PermissionDiscipline bo)
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
