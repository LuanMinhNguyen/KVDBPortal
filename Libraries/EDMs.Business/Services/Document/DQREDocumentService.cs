
namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.DAO.Scope;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class DQREDocumentService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DQREDocumentsDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="DQREDocumentService"/> class.
        /// </summary>
        public DQREDocumentService()
        {
            this.repo = new DQREDocumentsDAO();
        }

        #region Get (Advances)

        public DQREDocument GetOneByDocNo(string docNo, string revName, int projectcodeId)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault()
                    && t.DocumentNo.Trim().Replace(" ", string.Empty) == docNo.Trim().Replace(" ", string.Empty)
                    && t.Revision == revName
                    && t.ProjectCodeId == projectcodeId);
        }

        public List<DQREDocument> GetAllProjectCode(int projectcodeId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.ProjectCodeId == projectcodeId).ToList();
        }

        public List<DQREDocument> GetAllProject()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetAllDocumentReplease(string Originator = "DQRE")
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.M_OriginatorName.Trim()==Originator).ToList();
        }
        public List<DQREDocument> GetAllDocInWF()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsInWFProcess.GetValueOrDefault()
                && !t.IsWFComplete .GetValueOrDefault()).ToList();
        }

        public List<DQREDocument> GetAllByProjectDocNo(string projectDocNo)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.DocumentNo == projectDocNo).ToList();
        }

        /// <summary>
        /// The get all by package.
        /// </summary>
        /// <param name="packageId">
        /// The package id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DQREDocument> GetByDiscipline(int codeID)
        {
            return this.repo.GetByDiscipline(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetAllIncomming()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && !string.IsNullOrEmpty(t.IncomingTransNo)).ToList();
        }
        public List<DQREDocument> GetAllOverdueDocument()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && !string.IsNullOrEmpty(t.IncomingTransNo)
                && string.IsNullOrEmpty(t.OutgoingTransNo)).ToList();
        }
        public List<DQREDocument> GetAllOutGoing()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && !string.IsNullOrEmpty(t.OutgoingTransNo)).ToList();
        }
        public List<DQREDocument> GetByPlant(int PlantID)
        {
            return this.repo.GetByPlant(PlantID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByArea(int AreaID)
        {
            return this.repo.GetByArea(AreaID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByUnit(int codeID)
        {
            return this.repo.GetByUnit(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }

        public List<DQREDocument> GetByDocumentType(int codeID)
        {
            return this.repo.GetByDocumentType(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByMaterial(int codeID)
        {
            return this.repo.GetByMaterial(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByWork(int codeID)
        {
            return this.repo.GetByWork(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByDrawing(int codeID)
        {
            return this.repo.GetByDrawing(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList(); 
        }
        public List<DQREDocument> GetByOriginator(int OriginatorID)
        {
            return this.repo.GetByOriginator(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByOriginatingOrganization(int OriginatorID)
        {
            return this.repo.GetByOriginatingOrganization(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByReceivingOrganization(int OriginatorID)
        {
            return this.repo.GetByReceivingOrganization(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        public List<DQREDocument> GetByProjectCode(int projectID, bool isGetAll)
        {
            return this.repo.GetByProjectCode(projectID).Where(t=> !t.IsDelete.GetValueOrDefault()
                && (isGetAll || t.IsLeaf.GetValueOrDefault())).ToList();
        }
        public List<DQREDocument> GetByDepartment(string DepartmentCode)
        {
            return this.repo.GetByDepartment(DepartmentCode).Where(t=> !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault() ).ToList();
        }
        public bool IsExistByDocNo(string docNo)
        {
            return this.repo.GetAll().Any(t => t.IsLeaf.GetValueOrDefault() && !t.IsDelete.GetValueOrDefault() && t.DocumentNo == docNo);
        }

        /// <summary>
        /// The get all related document.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DQREDocument> GetAllRelatedDocument(Guid docId)
        {
            return this.repo.GetAll().Where(t => (t.ID == docId || t.ParentId == docId) && !t.IsDelete.GetValueOrDefault()).ToList();
        }

        public List<DQREDocument> GetAllRevDoc(Guid parentId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }



        public List<DQREDocument> SearchDocument(int projectId, int disciplineId, string docNo, string docTitle, string searchFullFields, bool isGetAllRevision)
        {
            return this.repo.GetAll().Where(
                t =>
                  !t.IsDelete.GetValueOrDefault()
                && (projectId == t.ProjectCodeId.GetValueOrDefault() || projectId == 0)
                && (disciplineId == 0 || t.M_DisciplineId.GetValueOrDefault() == disciplineId)
                && (string.IsNullOrEmpty(docNo) || t.DocumentNo.ToLower().Contains(docNo.ToLower()))
                && (string.IsNullOrEmpty(docTitle) || t.DocumentTitle.ToLower().Contains(docTitle.ToLower()))
                ).OrderBy(t => t.DocumentNo).ToList();
        }

        public List<DQREDocument> SearchDocument(string searchAll, string docNo, string title, int docTypeId, int projectId, int disciplineId, int revisonSchemaid, int engineerId)
        {
            return this.repo.GetAll().Where(
                t =>
                t.IsLeaf.GetValueOrDefault()
                && !t.IsDelete.GetValueOrDefault()
               
                && (string.IsNullOrEmpty(docNo) || t.DocumentNo.ToLower().Contains(docNo.ToLower()))
                && (string.IsNullOrEmpty(title) || t.DocumentTitle.ToLower().Contains(title.ToLower()))
                && (disciplineId == 0 || (t.M_DisciplineId != null && t.M_DisciplineId == disciplineId))
                && (projectId == 0 || (t.ProjectCodeId != null && t.ProjectCodeId == projectId))
                && (docTypeId == 0 || (t.M_DocumentTypeId != null && t.M_DocumentTypeId == docTypeId))
                && (revisonSchemaid == 0 || (t.RevisionSchemaId != null && t.RevisionSchemaId == revisonSchemaid))
               

                && (string.IsNullOrEmpty(searchAll)
                    || (!string.IsNullOrEmpty(t.DocumentNo) && t.DocumentNo.ToLower().Contains(searchAll.ToLower()))
                    || (!string.IsNullOrEmpty(t.DocumentTitle) && t.DocumentTitle.ToLower().Contains(searchAll.ToLower()))
                    || (!string.IsNullOrEmpty(t.ProjectCodeName) && t.ProjectCodeName.ToLower().Contains(searchAll.ToLower()))
                    || (!string.IsNullOrEmpty(t.M_DisciplineName) && t.M_DisciplineName.ToLower().Contains(searchAll.ToLower()))
                    )).OrderBy(t => t.DocumentNo).ToList();
        }

        public List<DQREDocument> GetAllDocList(List<Guid> listdoc)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.IsLeaf.GetValueOrDefault() && listdoc.Contains(t.ID)).ToList();
        }

        public List<DQREDocument> GetSpecialDocList(List<Guid> listdoc)
        {
            return this.repo.GetAll().Where(t => listdoc.Contains(t.ID)).ToList();
        }

        public List<DQREDocument> GetAllByMaster(Guid MasterId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.DocumentMasterId==MasterId).OrderBy(t=> t.CreatedDate).ToList();
        }
        public List<DQREDocument> GetAllByMaster(List<Guid> MasterId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && MasterId.Contains(t.DocumentMasterId.GetValueOrDefault())).ToList();
        }
        public bool IsExist(string documentNumber, Guid docId)
        {
            var docObj = this.GetById(docId);
            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.DocumentNo == documentNumber
                &&  (t.ID != docObj.ID
                        && t.ID != docObj.ParentId));
        }


        public bool IsExist(string documentNumber,string revname)
        {

            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.DocumentNo == documentNumber
                && t.Revision == revname.Trim());
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DQREDocument> GetAllLatest()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.IsLeaf.GetValueOrDefault()).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DQREDocument> GetAll()
        {
            return this.repo.GetAll().Where(t=> !t.IsDelete.GetValueOrDefault()).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DQREDocument GetById(Guid id)
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
        public Guid? Insert(DQREDocument bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DQREDocument bo)
        {
            try
            {
                return this.repo.Update(bo);
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
        public bool Delete(DQREDocument bo)
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
                return this.repo.Delete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion
    }
}
