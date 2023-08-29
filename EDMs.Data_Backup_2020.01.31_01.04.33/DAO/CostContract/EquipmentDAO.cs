
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentDAO.cs" company="">
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
    public class EquipmentDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentDAO"/> class.
        /// </summary>
        public EquipmentDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Equipment> GetIQueryable()
        {
            return this.EDMsDataContext.Equipments;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Equipment> GetAll()
        {
            return this.EDMsDataContext.Equipments.OrderByDescending(t => t.ID).ToList();
        }
       /// <summary>
       /// get of parrent id
       /// </summary>
       /// <param name="Id"></param>
       /// <returns></returns>
        public List<Equipment> GetAllParrentId( int Id)
        {
            return this.EDMsDataContext.Equipments.Where(ob=> ob.ParentId==Id).OrderByDescending(t => t.ID).ToList();
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
        public Equipment GetById(int id)
        {
            return this.EDMsDataContext.Equipments.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(Equipment ob)
        {
            try
            {
                this.EDMsDataContext.AddToEquipments(ob);
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
        public bool Update(Equipment src)
        {
            try
            {
                Equipment des = (from rs in this.EDMsDataContext.Equipments
                                              where rs.ID == src.ID
                                              select rs).First();

                des.Number = src.Number;
                des.EquipmentName = src.EquipmentName;
                des.ParentId = src.ParentId;
                des.ExpectedPrice = src.ExpectedPrice;
                des.Quantity = src.Quantity;
                des.CalculationUnit = src.CalculationUnit;
                des.Remark = src.Remark;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
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
        public bool Delete(Equipment src)
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
