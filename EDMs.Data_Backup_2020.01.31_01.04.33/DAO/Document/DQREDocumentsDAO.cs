 

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DQREConrespondenceDAO.cs" company="">
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
    public class DQREDocumentsDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DQREConrespondenceDAO"/> class.
        /// </summary>
        public DQREDocumentsDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DQREDocument> GetIQueryable()
        {
            return this.EDMsDataContext.DQREDocuments;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DQREDocument> GetAll()
        {
            return this.EDMsDataContext.DQREDocuments.OrderByDescending(t => t.ID).ToList();
        }

        public List<DQREDocument> GetByDiscipline(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_DisciplineId ==codeID )
                    .ToList();
        }

        public List<DQREDocument> GetByPlant(int PlantID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_PlantId == PlantID)
                    .ToList();
        }
        public List<DQREDocument> GetByArea(int AreaID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_AreaId == AreaID)
                    .ToList();
        }
        public List<DQREDocument> GetByUnit(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_UnitId == codeID)
                    .ToList();
        }

        public List<DQREDocument> GetByDocumentType(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_DocumentTypeId == codeID)
                    .ToList();
        }
        public List<DQREDocument> GetByMaterial(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_MaterialCodeId == codeID)
                    .ToList();
        }
        public List<DQREDocument> GetByWork(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_WorkCodeId == codeID)
                    .ToList();
        }
        public List<DQREDocument> GetByDrawing(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_DrawingCodeId == codeID)
                    .ToList();
        }
        public List<DQREDocument> GetByOriginator(int OriginatorID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_OriginatorId == OriginatorID)
                    .ToList();
        }
        public List<DQREDocument> GetByOriginatingOrganization(int OriginatorID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_OriginatingOrganizationId == OriginatorID)
                    .ToList();
        }
        public List<DQREDocument> GetByReceivingOrganization(int OriginatorID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_ReceivingOrganizationId == OriginatorID)
                    .ToList();
        }
        public List<DQREDocument> GetByProjectCode(int projectID)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>  t.ProjectCodeId ==projectID).ToList();
        }
        public List<DQREDocument> GetByDepartment(string DepartmentCode)
        {
            return
                this.EDMsDataContext.DQREDocuments.Where(t =>    t.M_DepartmentCode == DepartmentCode)
                    .ToList();
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
        public DQREDocument GetById(Guid id)
        {
            return this.EDMsDataContext.DQREDocuments.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(DQREDocument ob)
        {
            try
            {
                this.EDMsDataContext.AddToDQREDocuments(ob);
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
        public bool Update(DQREDocument src)
        {
            try
            {
                DQREDocument des = (from rs in this.EDMsDataContext.DQREDocuments
                                       where rs.ID == src.ID
                                       select rs).First();
                des.DocumentMasterId = src.DocumentMasterId;
                des.M_SystemDocumentNo = src.M_SystemDocumentNo;
                des.DocumentNo = src.DocumentNo;
                des.DocumentTitle = src.DocumentTitle;
                des.M_OriginatorId = src.M_OriginatorId;
                des.M_OriginatorName = src.M_OriginatorName;
                des.M_OriginatingOrganizationId = src.M_OriginatingOrganizationId;
                des.M_OriginatingOrganizationName = src.M_OriginatingOrganizationName;
                des.M_ReceivingOrganizationId = src.M_ReceivingOrganizationId;
                des.M_ReceivingOrganizationName = src.M_ReceivingOrganizationName;
                des.M_DocumentTypeId = src.M_DocumentTypeId;
                des.M_DocumentTypeName = src.M_DocumentTypeName;
                des.M_DisciplineId = src.M_DisciplineId;
                des.M_DisciplineName = src.M_DisciplineName;
                des.M_MaterialCodeId = src.M_MaterialCodeId;
                des.M_MaterialCodeName = src.M_MaterialCodeName;
                des.M_WorkCodeId = src.M_WorkCodeId;
                des.M_WorkCodeName = src.M_WorkCodeName;
                des.M_DrawingCodeId = src.M_DrawingCodeId;
                des.M_DrawingCodeName = src.M_DrawingCodeName;
                des.M_EquipmentTagName = src.M_EquipmentTagName;
                des.M_DepartmentCode = src.M_DepartmentCode;
                des.M_MRSequenceNo = src.M_MRSequenceNo;
                des.M_DocumentSequenceNo = src.M_DocumentSequenceNo;
                des.M_SheetNo = src.M_SheetNo;
                des.M_PlantId = src.M_PlantId;
                des.M_PlantName = src.M_PlantName;
                des.M_AreaId = src.M_AreaId;
                des.M_AreaName = src.M_AreaName;
                des.M_UnitId = src.M_UnitId;
                des.M_UnitName = src.M_UnitName;
                des.ProjectCodeId = src.ProjectCodeId;
                des.ProjectCodeName = src.ProjectCodeName;
                des.ContractorDocNo = src.ContractorDocNo;
                des.RevisionSchemaId = src.RevisionSchemaId;
                des.RevisionSchemaName = src.RevisionSchemaName;
                des.Revision = src.Revision;
                des.IsssuedDate = src.IsssuedDate;
                des.DocumentClassId = src.DocumentClassId;
                des.DocumentClassName = src.DocumentClassName;
                des.RevisionStatusId = src.RevisionStatusId;
                des.RevisionStatusName = src.RevisionStatusName;
                des.DocumentCodeId = src.DocumentCodeId;
                des.DocumentCodeName = src.DocumentCodeName;
                des.Remark = src.Remark;
                des.ConfidentialityId = src.ConfidentialityId;
                des.ConfidentialityName = src.ConfidentialityName;
                des.ParentId = src.ParentId;
                des.IsDelete = src.IsDelete;
                des.IsLeaf = src.IsLeaf;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.RevisionState = src.RevisionState;

                des.IncomingTransId = src.IncomingTransId;
                des.IncomingTransNo = src.IncomingTransNo;
                des.OutgoingTransId = src.OutgoingTransId;
                des.OutgoingTransNo = src.OutgoingTransNo;
                des.IsCreateOutgoingTrans = src.IsCreateOutgoingTrans;
                des.CreatedByName = src.CreatedByName;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.IsHasAttachFile = src.IsHasAttachFile;
                des.SerialNo = src.SerialNo;
                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
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
        public bool Delete(DQREDocument src)
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
