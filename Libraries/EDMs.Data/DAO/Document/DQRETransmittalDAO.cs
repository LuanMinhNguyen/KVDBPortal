// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DQRETransmittalDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class DQRETransmittalDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DQRETransmittalDAO"/> class.
        /// </summary>
        public DQRETransmittalDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DQRETransmittal> GetIQueryable()
        {
            return this.EDMsDataContext.DQRETransmittals;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DQRETransmittal> GetAll()
        {
            return this.EDMsDataContext.DQRETransmittals.ToList();
        }

        /// <summary>
        /// The get all by owner.
        /// </summary>
        /// <param name="createdBy">
        /// The created by.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //public List<DQRETransmittal> GetAllByOwner(int userId)
        //{
        //    return this.EDMsDataContext.DQRETransmittals.Where(t => t.CreatedBy == userId || t.FromId == userId || t.ToId == userId).ToList();
        //}

        /// <summary>
        /// The get by id.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public DQRETransmittal GetById(Guid id)
        {
            return this.EDMsDataContext.DQRETransmittals.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get specific.
        /// </summary>
        /// <param name="tranId">
        /// The tran id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        //public List<DQRETransmittal> GetSpecific(int tranId)
        //{
        //    return this.EDMsDataContext.DQRETransmittals.Where(t => t.ID == tranId).ToList();
        //}
        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="ob">
        /// The ob.
        /// </param>
        /// <returns>
        /// The <see>
        ///       <cref>int?</cref>
        ///     </see> .
        /// </returns>
        public Guid? Insert(DQRETransmittal ob)
        {
            try
            {
                this.EDMsDataContext.AddToDQRETransmittals(ob);
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
        public bool Update(DQRETransmittal src)
        {
            try
            {
                DQRETransmittal des = (from rs in this.EDMsDataContext.DQRETransmittals
                                where rs.ID == src.ID
                                select rs).First();

                des.TransmittalNo = src.TransmittalNo;
                des.Description = src.Description;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
                des.ReceivingOrganizationId = src.ReceivingOrganizationId;
                des.ReceivingOrganizationName = src.ReceivingOrganizationName;
                des.ProjectCodeId = src.ProjectCodeId;
                des.ProjectCodeName = src.ProjectCodeName;
                des.IssuedDate = src.IssuedDate;
                des.ReceivedDate = src.ReceivedDate;
                des.Remark = src.Remark;
                des.ConfidentialityId = src.ConfidentialityId;
                des.ConfidentialityName = src.ConfidentialityName;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;
                des.File = src.File;
                des.HasAttachFile = src.HasAttachFile;
                des.CreatedBy = src.CreatedBy;
                des.CreatedDate = src.CreatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;

                des.TypeId = src.TypeId;
                des.StoreFolderPath = src.StoreFolderPath;
                des.Status = src.Status;
                des.IsOpen = src.IsOpen;
                des.IsImport = src.IsImport;
                des.ContractorTransId = src.ContractorTransId;
                des.IsSend = src.IsSend;
                des.ErrorMessage = src.ErrorMessage;
                des.IsValid = src.IsValid;
                des.DueDate = src.DueDate;

                des.RefTransId = src.RefTransId;
                des.RefTransNo = src.RefTransNo;
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
        public bool Delete(DQRETransmittal src)
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
