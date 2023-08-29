using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Business.Services.Security;
using EDMs.Data.DAO.WMS;
using EDMs.Data.Entities;

namespace EDMs.Business.Services.WMS
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class PerformanceReviewAttachFileService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PerformanceReviewAttachFileDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="PerformanceReviewAttachFileService"/> class.
        /// </summary>
        public PerformanceReviewAttachFileService()
        {
            this.repo = new PerformanceReviewAttachFileDAO();

        }

        #region Get (Advances)
        public List<PerformanceReviewAttachFile> GetAllObjId(int objId)
        {
            return this.repo.GetAll().Where(t => t.UserId == objId).OrderBy(t => t.CreatedDate).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PerformanceReviewAttachFile> GetAll()
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
        public PerformanceReviewAttachFile GetById(Guid id)
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
        public Guid? Insert(PerformanceReviewAttachFile bo)
        {
            var objId = this.repo.Insert(bo);
            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(PerformanceReviewAttachFile bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                return flag;
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
        public bool Delete(PerformanceReviewAttachFile bo)
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
                var flag = this.repo.Delete(id);

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
