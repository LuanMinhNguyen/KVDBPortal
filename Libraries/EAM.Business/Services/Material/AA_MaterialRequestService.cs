using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.DAO.Material;
using EAM.Data.Dto;
using EAM.Data.Entities;

namespace EAM.Business.Services.Material
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class AA_MaterialRequestService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly AA_MaterialRequestDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="AA_MaterialRequestService"/> class.
        /// </summary>
        public AA_MaterialRequestService()
        {
            this.repo = new AA_MaterialRequestDAO();

        }

        #region Get (Advances)
        public List<AA_MaterialRequest> GetMRList(UserInforDto userInfor, string authGroup)
        {
            return this.repo.GetAll()
                .Where(t => userInfor.MANHOM.ToUpper() == "ADMIN"
                            || userInfor.MANHOM.ToUpper() == "1"
                            || t.CreatedById == userInfor.MANGUOIDUNG
                            || (t.StatusId > 1 && userInfor.MANHOM == authGroup)
                            )
                .OrderByDescending(t => t.Code).ToList();
        }

        public List<AA_MaterialRequest> GetAllMRWaitingApprove()
        {
            return this.repo.GetAll()
                .Where(t => t.StatusId == 2)
                .OrderByDescending(t => t.Code).ToList();
        }

        public List<AA_MaterialRequest> Search(UserInforDto userInfor, string authGroup, string searchText,
            bool isDinhKy)
        {
            return this.GetMRList(userInfor, authGroup)
                .Where(t => (string.IsNullOrEmpty(searchText)
                            || t.Code.ToString().Contains(searchText)
                            || t.Description.ToUpper().Contains(searchText)
                            || t.OrganizationCode.ToUpper().Contains(searchText)
                            || t.OrganizationName.ToUpper().Contains(searchText)
                            || t.StoreCode.ToUpper().Contains(searchText)
                            || t.StoreName.ToUpper().Contains(searchText)
                            || t.RequestBy.ToUpper().Contains(searchText)
                            || t.Note.ToUpper().Contains(searchText)
                            || t.StatusName.ToUpper().Contains(searchText))
                            && t.IsMonthlyRequest == isDinhKy
                )
                .OrderByDescending(t => t.Code).ToList();
        }

        public List<AA_MaterialRequest> SearchAll(UserInforDto userInfor, string authGroup, string searchText)
        {
            return this.GetMRList(userInfor, authGroup)
                .Where(t => (string.IsNullOrEmpty(searchText)
                             || t.Code.ToString().Contains(searchText)
                             || t.Description.ToUpper().Contains(searchText)
                             || t.OrganizationCode.ToUpper().Contains(searchText)
                             || t.OrganizationName.ToUpper().Contains(searchText)
                             || t.StoreCode.ToUpper().Contains(searchText)
                             || t.StoreName.ToUpper().Contains(searchText)
                             || t.RequestBy.ToUpper().Contains(searchText)
                             || t.Note.ToUpper().Contains(searchText)
                             || t.StatusName.ToUpper().Contains(searchText))
                )
                .OrderByDescending(t => t.Code).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<AA_MaterialRequest> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.Code).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public AA_MaterialRequest GetById(Guid id)
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
        public Guid? Insert(AA_MaterialRequest bo)
        {
            var objId = this.repo.Insert(bo);
            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(AA_MaterialRequest bo)
        {
            try
            {
                var flag = this.repo.Update(bo);
                //if (flag)
                //{
                //    var changeData = new WaitingSyncData()
                //    {
                //        ActionTypeID = 2,
                //        ActionTypeName = "Update",
                //        ObjectID2 = bo.ID,
                //        ObjectName = "[WMS].[AA_MaterialRequest]",
                //        EffectDate = DateTime.Now,IsSynced = false
                //    };

                //    this.waitingSyncDataService.Insert(changeData);
                //}

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
        public bool Delete(AA_MaterialRequest bo)
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
                //if (flag)
                //{
                //    // Trigger data change
                //    var changeData = new WaitingSyncData()
                //    {
                //        ActionTypeID = 3,
                //        ActionTypeName = "Delete",
                //        ObjectID2 = id,
                //        ObjectName = "[WMS].[AA_MaterialRequest]",
                //        EffectDate = DateTime.Now,IsSynced = false
                //    };

                //    this.waitingSyncDataService.Insert(changeData);
                //    // ----------------------------------------------------------------------
                //}

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
