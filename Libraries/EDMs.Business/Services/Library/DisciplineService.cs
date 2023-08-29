using EDMs.Business.Services.Security;

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
    public class DisciplineService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DisciplineDAO repo;

        private readonly PermissionDisciplineService permissionDisciplineService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisciplineService"/> class.
        /// </summary>
        public DisciplineService()
        {
            this.repo = new DisciplineDAO();
            this.permissionDisciplineService = new PermissionDisciplineService();
        }

        #region Get (Advances)
        public List<Discipline> GetAllDisciplineOfProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public List<Discipline> GetAllDisciplineInPermission(int userId, int projectId)
        {
            var workGroupIdInPermission = this.permissionDisciplineService.GetDisciplineInPermission(userId);
            return this.repo.GetAll().Where(t => t.ProjectId == projectId && workGroupIdInPermission.Contains(t.ID)).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<Discipline> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<Discipline> GetAllByCategory(List<int> categoryIds)
        {
            return this.repo.GetAllByCategory(categoryIds);
        }

        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Discipline> GetAllByCategory(int categoryId)
        {
            return this.repo.GetAllByCategory(categoryId);
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public Discipline GetById(int id)
        {
            return this.repo.GetById(id);
        }

        public Discipline GetByCode(string code)
        {
            return this.repo.GetByCode(code);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(Discipline bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Discipline bo)
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
        public bool Delete(Discipline bo)
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

        public Discipline GetByName(string name, int projectId)
        {
            return this.repo.GetByName(name, projectId);
        }
    }
}
