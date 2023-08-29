// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MaterialDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.CostContract
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class MaterialDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaterialDAO"/> class.
        /// </summary>
        public MaterialDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Material> GetIQueryable()
        {
            return this.EDMsDataContext.Materials;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Material> GetAll()
        {
            return this.EDMsDataContext.Materials.OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public Material GetById(int id)
        {
            return this.EDMsDataContext.Materials.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(Material ob)
        {
            try
            {
                this.EDMsDataContext.AddToMaterials(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="src">
        /// Entity for update
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if update success, false if not
        /// </returns>
        public bool Update(Material src)
        {
            try
            {
                Material des = (from rs in this.EDMsDataContext.Materials
                                where rs.ID == src.ID
                                select rs).First();

                des.Number = src.Number;
                des.ContractID = src.ContractID;
                des.ContractNumber = src.ContractNumber;
                des.Number = src.Number;
                des.Quality = src.Quality;
                des.DeliveryActual = src.DeliveryActual;
                des.DeliveryPlan = src.DeliveryPlan;
                des.MaterialTypeID = src.MaterialTypeID;
                des.MaterialTypeName = src.MaterialTypeName;
                des.Weight = src.Weight;
                des.Complete = src.Complete;
                des.CompleteContract = src.CompleteContract;
                des.UnitID = src.UnitID;
                des.UnitName = src.UnitName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;

                des.DeliveryStatus = src.DeliveryStatus;
                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
                des.PRID = src.PRID;
                des.PRNumber = src.PRNumber;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="src">
        /// The src.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// True if delete success, false if not
        /// </returns>
        public bool Delete(Material src)
        {
            try
            {
                var des = this.GetById(src.ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="ID"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                var des = this.GetById(ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
        #endregion
    }
}
