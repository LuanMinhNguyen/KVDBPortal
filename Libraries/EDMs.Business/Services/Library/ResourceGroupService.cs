namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Library;

    public class ResourceGroupService
    {     
       private readonly ResourceGroupDAO repo;

        public ResourceGroupService()
        {
            this.repo = new ResourceGroupDAO();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All ResourceGroup
        /// </summary>
        /// <returns></returns>
        public List<ResourceGroup> GetAll()
        {
            return this.repo.GetAll().ToList();
            //return patientDAO.GetAll();
        }

        /// <summary>
        /// Get ResourceGroup By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ResourceGroup GetByID(int ID)
        {
            return this.repo.GetByID( ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert ResourceGroup
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Insert(ResourceGroup bo)
        {
            try
            {
                return this.repo.Insert(bo);             
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update ResourceGroup
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(ResourceGroup bo)
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
        /// Delete ResourceGroup
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(ResourceGroup bo)
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
        /// Delete ResourceGroup By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                return this.repo.Delete(ID);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
