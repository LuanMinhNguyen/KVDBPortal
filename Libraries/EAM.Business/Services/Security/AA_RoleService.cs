using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.DAO.Security;
using EAM.Data.Entities;

namespace EAM.Business.Services.Security
{
    public class AA_RolesService
    {      
        private readonly AA_RolesDAO repo;

        //private readonly WaitingSyncDataService waitingSyncDataService;

        public AA_RolesService()
        {
            this.repo = new AA_RolesDAO();
            //this.waitingSyncDataService = new WaitingSyncDataService();
        }
        #region GET (Basic)
        /// <summary>
        /// Get All AA_Roles
        /// </summary>
        /// <param name="isHost">
        /// The is Host.
        /// </param>
        /// <returns>
        /// </returns>
        public List<AA_Roles> GetAll(bool isHost)
        {
            return isHost
                       ? this.repo.GetAll()
                       : this.repo.GetAll().Where(t => t.Id != 1 && !t.IsAdmin.GetValueOrDefault()).ToList();
        }

        /// <summary>
        /// Get All AA_Roles
        /// </summary>
        /// <returns></returns>
        public List<AA_Roles> GetAllSpecial()
        {
            return this.repo.GetAllSpecial();
        }

        /// <summary>
        /// Get AA_Roles By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public AA_Roles GetByID(int ID)
        {
            return this.repo.GetByID( ID);
            //return patientDAO.GetByID(ID);
        }
        #endregion

        #region Get (Advances)

        public AA_Roles GetByName(string name)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim());
        }

        public AA_Roles GetByContractor(int id)
        {
            return this.repo.GetAll().FirstOrDefault(t =>t.ContractorId!= null && t.ContractorId==id);
        }

        #endregion

        #region Insert, Update, Delete
        /// <summary>
        /// Insert AA_Roles
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Insert(AA_Roles bo)
        {
            try
            {
                var flag = this.repo.Insert(bo);
                // Trigger data change
                ////if (flag != null)
                ////{
                ////    var changeData = new WaitingSyncData()
                ////    {
                ////        ActionTypeID = 1,
                ////        ActionTypeName = "Insert",
                ////        ObjectID2 = bo.Id,
                ////        ObjectName = "[Security].[AA_Roless]",
                ////        EffectDate = DateTime.Now,IsSynced = false
                ////    };

                ////    this.waitingSyncDataService.Insert(changeData);
                ////}

                // -------------------------------------------------------

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update AA_Roles
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(AA_Roles bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                ////if (flag)
                ////{
                ////    var changeData = new WaitingSyncData()
                ////    {
                ////        ActionTypeID = 2,
                ////        ActionTypeName = "Update",
                ////        ObjectID2 = bo.Id,
                ////        ObjectName = "[Security].[AA_Roless]",
                ////        EffectDate = DateTime.Now,IsSynced = false
                ////    };

                ////    this.waitingSyncDataService.Insert(changeData);
                ////}

                return flag;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Delete AA_Roles
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Delete(AA_Roles bo)
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
        /// Delete AA_Roles By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var flag = this.repo.Delete(id);
                ////if (flag)
                ////{
                ////    // Trigger data change
                ////    var changeData = new WaitingSyncData()
                ////    {
                ////        ActionTypeID = 3,
                ////        ActionTypeName = "Delete",
                ////        ObjectID2 = id,
                ////        ObjectName = "[Security].[AA_Roless]",
                ////        EffectDate = DateTime.Now,IsSynced = false
                ////    };

                ////    this.waitingSyncDataService.Insert(changeData);
                ////    // ----------------------------------------------------------------------
                ////}

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
