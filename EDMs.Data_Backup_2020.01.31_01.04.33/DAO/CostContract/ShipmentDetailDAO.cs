﻿
using System.Collections.Generic;
using System.Linq;
using System;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.CostContract
{
    public class ShipmentDetailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentDetailDAO"/> class.
        /// </summary>
        public ShipmentDetailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ShipmentDetail> GetIQueryable()
        {
            return this.EDMsDataContext.ShipmentDetails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ShipmentDetail> GetAll()
        {
            return this.EDMsDataContext.ShipmentDetails.OrderByDescending(t => t.ID).ToList();
        }
        /// <summary>
        ///  get all document of shipment
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<ShipmentDetail> GetAllShipmentId(Guid ID)
        {
            return this.EDMsDataContext.ShipmentDetails.Where(ob=> ob.ShipmentID == ID).OrderByDescending(t => t.ID).ToList();
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
        public ShipmentDetail GetById(int id)
        {
            return this.EDMsDataContext.ShipmentDetails.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(ShipmentDetail ob)
        {
            try
            {
                this.EDMsDataContext.AddToShipmentDetails(ob);
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
        public bool Update(ShipmentDetail src)
        {
            try
            {
                ShipmentDetail des = (from rs in this.EDMsDataContext.ShipmentDetails
                                where rs.ID == src.ID
                                select rs).First();

                des.ShipmentID = src.ShipmentID;
                des.ShipmentNumber = src.ShipmentNumber;
                des.KKSId = src.KKSId;
                des.KKSCode = src.KKSCode;
                des.EquipmentID = src.EquipmentID;
                des.EquipmentNumber = src.EquipmentNumber;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.Quantity = src.Quantity;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
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
        public bool Delete(ShipmentDetail src)
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
