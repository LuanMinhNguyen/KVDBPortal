
using System.Collections.Generic;
using System.Linq;
using System;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.CostContract
{
    public class ShipmentDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentDAO"/> class.
        /// </summary>
        public ShipmentDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Shipment> GetIQueryable()
        {
            return this.EDMsDataContext.Shipments;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Shipment> GetAll()
        {
            return this.EDMsDataContext.Shipments.OrderByDescending(t => t.ID).ToList();
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
        public Shipment GetById(Guid id)
        {
            return this.EDMsDataContext.Shipments.FirstOrDefault(ob => ob.ID == id);
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
        public bool Insert(Shipment ob)
        {
            try
            {
                this.EDMsDataContext.AddToShipments(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
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
        public bool Update(Shipment src)
        {
            try
            {
                Shipment des = (from rs in this.EDMsDataContext.Shipments
                                 where rs.ID == src.ID
                                 select rs).First();

                des.Number = src.Number;
                des.Description = src.Description;
                des.Date = src.Date;
                des.ShipmentStatusID = src.ShipmentStatusID;
                des.ShipmentStatusName = src.ShipmentStatusName;
                des.ShipmentTypeId = src.ShipmentTypeId;
                des.ShipmentTypeName = src.ShipmentTypeName;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.DefaultFilePath = src.DefaultFilePath;
                des.FileExtentionIcon = src.FileExtentionIcon;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;

                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.IsHasAttachFile = src.IsHasAttachFile;
                des.IsSend = src.IsSend;
                des.IsUseCustomWfFromTrans = src.IsUseCustomWfFromTrans;
                des.IsUseIsUseCustomWfFromObj = src.IsUseIsUseCustomWfFromObj;
                des.IsAttachWorkflow = src.IsAttachWorkflow;

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
        public bool Delete(Shipment src)
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
        public bool Delete(Guid ID)
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
