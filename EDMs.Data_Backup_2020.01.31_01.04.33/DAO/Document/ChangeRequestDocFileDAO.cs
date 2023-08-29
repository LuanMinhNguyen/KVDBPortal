﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeRequestDocFileDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class ChangeRequestDocFileDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeRequestDocFileDAO"/> class.
        /// </summary>
        public ChangeRequestDocFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ChangeRequestDocFile> GetIQueryable()
        {
            return this.EDMsDataContext.ChangeRequestDocFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ChangeRequestDocFile> GetAll()
        {
            return this.EDMsDataContext.ChangeRequestDocFiles.ToList();
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
        public ChangeRequestDocFile GetById(Guid id)
        {
            return this.EDMsDataContext.ChangeRequestDocFiles.FirstOrDefault(ob => ob.ID == id);
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
        public List<ChangeRequestDocFile> GetSpecific(Guid changeRequestId)
        {
            return this.EDMsDataContext.ChangeRequestDocFiles.Where(t => t.ChangeRequestId == changeRequestId).ToList();
        }
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
        public Guid? Insert(ChangeRequestDocFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToChangeRequestDocFiles(ob);
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
        public bool Update(ChangeRequestDocFile src)
        {
            try
            {
                ChangeRequestDocFile des = (from rs in this.EDMsDataContext.ChangeRequestDocFiles
                                where rs.ID == src.ID
                                select rs).First();

                des.FileName = src.FileName;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
                des.ExtensionIcon = src.ExtensionIcon;
                des.FileSize = src.FileSize;
                des.DocumentNo = src.DocumentNo;
                des.DocumentNoFull = src.DocumentNoFull;
                des.Revision = src.Revision;
                des.IssueDate = src.IssueDate;
                des.DocumentTitle = src.DocumentTitle;
                des.DocumentClassId = src.DocumentClassId;
                des.DocumentClassName = src.DocumentClassName;
                des.DocumentCodeId = src.DocumentCodeId;
                des.DocumentCodeName = src.DocumentCodeName;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.AreaId = src.AreaId;
                des.AreaName = src.AreaName;
                des.UnitCodeId = src.UnitCodeId;
                des.UnitCodeName = src.UnitCodeName;
                des.DocumentTypeId = src.DocumentTypeId;
                des.DocumentTypeName = src.DocumentTypeName;
                des.DisciplineCodeId = src.DisciplineCodeId;
                des.DisciplineCodeName = src.DisciplineCodeName;
                des.MaterialCodeId = src.MaterialCodeId;
                des.MaterialCodeName = src.MaterialCodeName;
                des.MaterialRequisition = src.MaterialRequisition;
                des.WorkCodeId = src.WorkCodeId;
                des.WorkCodeName = src.WorkCodeName;
                des.DrawingCodeId = src.DrawingCodeId;
                des.DrawingCodeName = src.DrawingCodeName;
                des.DepartmentCode = src.DepartmentCode;
                des.EquipmentTagNo = src.EquipmentTagNo;
                des.SerialNo = src.SerialNo;
                des.PONo = src.PONo;
                des.Sequence = src.Sequence;
                des.SequenceOfFile = src.SequenceOfFile;

                des.ContractorRefNo = src.ContractorRefNo;
                des.DocumentTypeGroupId = src.DocumentTypeGroupId;
                des.DocumentTypeGroupName = src.DocumentTypeGroupName;

                des.Status = src.Status;
                des.ErrorMessage = src.ErrorMessage;
                des.ErrorPosition = src.ErrorPosition;

                des.IsReject = src.IsReject;
                des.RejectReason = src.RejectReason;
                des.PECC2ProjectDocId = src.PECC2ProjectDocId;
                des.DQREProjectDocId = src.DQREProjectDocId;
                des.IsAttachWorkflow = src.IsAttachWorkflow;

                des.KKSCodeId = src.KKSCodeId;
                des.KKSCodeName = src.KKSCodeName;
                des.TrainNo = src.TrainNo;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
                des.ReceivingOrganizationId = src.ReceivingOrganizationId;
                des.ReceivingOrganizationName = src.ReceivingOrganizationName;
                des.GroupCodeId = src.GroupCodeId;
                des.GroupCodeName = src.GroupCodeName;
                des.ChangeRequestTypeId = src.ChangeRequestTypeId;
                des.ChangeRequestTypeName = src.ChangeRequestTypeName;
                des.Year = src.Year;
                des.PurposeId = src.PurposeId;
                des.PurposeName = src.PurposeName;
                des.RevRemark = src.RevRemark;
                des.Pecc2ReviewStatusId = src.Pecc2ReviewStatusId;
                des.Pecc2ReviewStatusName = src.Pecc2ReviewStatusName;
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
        public bool Delete(ChangeRequestDocFile src)
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
