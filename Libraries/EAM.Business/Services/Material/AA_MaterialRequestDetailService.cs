using System;
using System.Collections.Generic;
using System.Linq;
using EAM.Data.DAO.Material;
using EAM.Data.Entities;

namespace EAM.Business.Services.Material
{
    /// <summary>
    /// The category service.
    /// </summary>
    public class AA_MaterialRequestDetailService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly AA_MaterialRequestDetailDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="AA_MaterialRequestDetailService"/> class.
        /// </summary>
        public AA_MaterialRequestDetailService()
        {
            this.repo = new AA_MaterialRequestDetailDAO();

        }

        #region Get (Advances)
        public List<AA_MaterialRequestDetail> GetAllByMR(Guid mrId)
        {
            return this.repo.GetAll().Where(t => t.MaterialRequestID == mrId).OrderBy(t => t.PartCode).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<AA_MaterialRequestDetail> GetAll()
        {
            return this.repo.GetAll().OrderBy(t => t.PartCode).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public AA_MaterialRequestDetail GetById(Guid id)
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
        public Guid? Insert(AA_MaterialRequestDetail bo)
        {
            var objId = this.repo.Insert(bo);
            return objId;
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(AA_MaterialRequestDetail bo)
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
                //        ObjectName = "[WMS].[AA_MaterialRequestDetail]",
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
        public bool Delete(AA_MaterialRequestDetail bo)
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
                //        ObjectName = "[WMS].[AA_MaterialRequestDetail]",
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
