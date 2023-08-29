namespace EDMs.Business.Services.Library
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Library;

    public class ResourceService
    {
        private readonly ResourceDAO repo;

        public ResourceService()
        {
            this.repo = new ResourceDAO();
        }

        #region Get (Advances)
        /// <summary>
        /// Get All Patient
        /// </summary>
        /// <returns>
        /// List patient
        /// </returns>
        public List<Resource> GetByResourceGroup(int resourceGroupId)
        {
            return this.repo.GetByResourceGroup(resourceGroupId);
        }

        public List<Resource> FindByFullName(string fullName, bool isResource = false)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                if (isResource)
                    return this.repo.GetAllIsResource();
                else
                    return this.repo.GetAll();
            }

            if (isResource)
                return this.repo.GetByFullNameIsResource(fullName);
            else
                return this.repo.GetByFullName(fullName);
        }

        public List<Resource> FindByFullName(string fullName, int resourceGroupId, bool isResource = false)
        {
            if (string.IsNullOrEmpty(fullName))
               return this.repo.GetByResourceGroup(resourceGroupId);

            if (isResource)
                return this.repo.GetByFullNameIsResource(fullName, resourceGroupId);
            else
                return this.repo.GetByFullName(fullName, resourceGroupId);
        }

        public List<Resource> FindByIsResource(bool isResource)
        {
            return this.repo.GetByIsResource(isResource);
        }

        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Resource
        /// </summary>
        /// <returns></returns>
        public List<Resource> GetAll(int pageSize, int startingRecordNumber, bool isResource = false)
        {
            if (isResource)
            {
                return this.repo.GetAllIsResource();
            }
            
            return this.repo.GetAll(pageSize, startingRecordNumber).ToList();
        }

        public List<Resource> GetAll(bool isResource = false)
        {
            if (isResource)
            {
                return this.repo.GetAllIsResource();
            }

            return this.repo.GetAll().ToList();
        }

        public int GetItemCount()
        {
            return this.repo.GetItemCount();
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Resource GetByID(int ID)
        {
            return this.repo.GetByID(ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public int? Insert(Resource bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Resource bo)
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
        public bool Delete(Resource bo)
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
