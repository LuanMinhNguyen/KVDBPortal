// <copyright file="" company="">
//   
// </copyright>
// <summary>
//   The category service.
// </summary>
// ---------------

namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.Entities;
    public   class RFIService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly RFIDAO repo;

       

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalService"/> class.
        /// </summary>
        public RFIService()
        {
            this.repo = new RFIDAO();
          
        }

        #region Get (Advances)

        public bool IsExist(string number)
        {
            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.Number == number);
        }


        public int GetCurrentSequence(int year, int group)
        {
            var result = 1;
            var currentSequence = this.repo.GetAll().Where(t =>!t.IsDelete.GetValueOrDefault() && t.GroupId==group&& t.Year == year).Max(t => t.Sequence);
            if (currentSequence != null)
            {
                result = currentSequence.Value + 1;
            }

            return result;
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<RFI> GetAll()
        {
            return this.repo.GetAll().ToList();
        }

        public List<RFI> GetAllByProject(int projectId)
        {
            return this.repo.GetAll().Where(t => t.ProjectId == projectId).ToList();
        }

        public RFI GetByNumber(string number)
        {
            return this.repo.GetByNumber(number);
        }
        public List<RFI> GetAllByIssueDate(DateTime isuedate)
        {
            return this.repo.GetByIssudDate(isuedate).ToList();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public RFI GetById(Guid id)
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
        public Guid? Insert(RFI bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(RFI bo)
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
        public bool Delete(RFI bo)
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
