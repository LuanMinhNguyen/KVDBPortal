namespace EDMs.Business.Services.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Security;

    public class RoleService
    {      
        private readonly RoleDAO repo;

        private readonly WaitingSyncDataService waitingSyncDataService;

        public RoleService()
        {
            this.repo = new RoleDAO();
            this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All Role
        /// </summary>
        /// <param name="isHost">
        /// The is Host.
        /// </param>
        /// <returns>
        /// </returns>
        public List<Role> GetAll(bool isHost)
        {
            return isHost
                       ? this.repo.GetAll()
                       : this.repo.GetAll().Where(t => t.Id != 1 && !t.IsAdmin.GetValueOrDefault()).ToList();
        }

        /// <summary>
        /// Get All Role
        /// </summary>
        /// <returns></returns>
        public List<Role> GetAllSpecial()
        {
            return this.repo.GetAllSpecial();
        }

        /// <summary>
        /// Get Role By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public Role GetByID(int ID)
        {
            return this.repo.GetByID( ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public Role GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim());
        }

        public Role GetByContractor(int id)
        {
            return this.repo.GetAll().FirstOrDefault(t =>t.ContractorId!= null && t.ContractorId==id);
        }

        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert Role
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Insert(Role bo)
        {
            try
            {
                var flag = this.repo.Insert(bo);
                // Trigger data change
                if (flag != null)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 1,
                        ActionTypeName = "Insert",
                        ObjectID2 = bo.Id,
                        ObjectName = "[Security].[Roles]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

                // -------------------------------------------------------

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update Role
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(Role bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                if (flag)
                {
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 2,
                        ActionTypeName = "Update",
                        ObjectID2 = bo.Id,
                        ObjectName = "[Security].[Roles]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                }

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete Role
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(Role bo)
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
        /// Delete Role By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var flag = this.repo.Delete(id);
                if (flag)
                {
                    // Trigger data change
                    var changeData = new WaitingSyncData()
                    {
                        ActionTypeID = 3,
                        ActionTypeName = "Delete",
                        ObjectID2 = id,
                        ObjectName = "[Security].[Roles]",
                        EffectDate = DateTime.Now,IsSynced = false
                    };

                    this.waitingSyncDataService.Insert(changeData);
                    // ----------------------------------------------------------------------
                }

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
