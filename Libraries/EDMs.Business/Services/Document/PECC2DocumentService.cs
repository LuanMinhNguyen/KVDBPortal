
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
    public class PECC2DocumentsService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly PECC2DocumentsDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2DocumentsService"/> class.
        /// </summary>
        public PECC2DocumentsService()
        {
            this.repo = new PECC2DocumentsDAO();
        }

        #region Get (Advances)

        public List<PECC2Documents> GetByProjectCode(int projectId, int confidential, bool isleaf)
        {
            return this.repo.GetByProjectCode(projectId).Where(t => !t.IsDelete.GetValueOrDefault()
                && t.ConfidentialityId <= confidential
                &&(isleaf || t.IsLeaf.GetValueOrDefault())).ToList();
        }


        public List<PECC2Documents> GetByProjectCode(int projectId, bool isGetAll, int categoryId)
        {
            return this.repo.GetByProjectCode(projectId).Where(t => !t.IsDelete.GetValueOrDefault()
                && (isGetAll || t.IsLeaf.GetValueOrDefault())
                && t.CategoryId == categoryId).ToList();
        }

        public PECC2Documents GetOneByDocNo(string docNo, string revName, int projectcodeId)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault()
                    && t.DocNo.Trim().Replace(" ", string.Empty) == docNo.Trim().Replace(" ", string.Empty)
                    && t.Revision == revName
                    && t.ProjectId == projectcodeId);
        }

        public PECC2Documents GetOneByDocNo(string docNo)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault()
                    && t.DocNo.Trim().Replace(" ", string.Empty) == docNo.Trim().Replace(" ", string.Empty));
        }

        public PECC2Documents GetOneByDocNoRev(string docNo, string rev)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault()
                    && t.DocNo.Trim().Replace(" ", string.Empty) == docNo.Trim().Replace(" ", string.Empty)
                    && t.Revision == rev);
        }

        public List<PECC2Documents> GetAllProjectCode(int projectcodeId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && t.ProjectId == projectcodeId).ToList();
        }

        public List<PECC2Documents> GetAllByIncomingTrans(Guid incomingTransId)
        {
            return this.repo.GetAll().Where(t => t.IncomingTransId == incomingTransId && !t.IsDelete.GetValueOrDefault()).ToList();
        }

        public List<PECC2Documents> GetAllByReferChangeRequest(Guid changerequestId)
        {
            return this.GetAll().Where(t => t.ChangeRequestId == changerequestId).ToList();
        }
        public List<PECC2Documents> GetAllByRevisedChangeRequest(Guid changerequestId)
        {
            return this.GetAll().Where(t => t.ChangeRequestIdFoRevised == changerequestId).ToList();
        }
        public List<PECC2Documents> GetAllByOutgoingTrans(Guid outgoingTransId)
        {
            return this.GetAll().Where(t => t.OutgoingTransId == outgoingTransId).ToList();
        }

        public List<PECC2Documents> GetAllProject()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()).ToList();
        }

        public List<PECC2Documents> GetAllProject(int categoryId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.CategoryId == categoryId).ToList();
        }
        //public List<PECC2Documents> GetAllDocumentReplease(string Originator = "DQRE")
        //{
        //    return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.M_OriginatorName.Trim()==Originator).ToList();
        //}
        public List<PECC2Documents> GetAllDocInWF()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsInWFProcess.GetValueOrDefault()
                && !t.IsWFComplete .GetValueOrDefault()).ToList();
        }

        public List<PECC2Documents> GetAllByProjectDocNo(string projectDocNo)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.DocNo == projectDocNo).ToList();
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
        //public List<PECC2Documents> GetByDiscipline(int codeID)
        //{
        //    return this.repo.GetByDiscipline(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        public List<PECC2Documents> GetAllIncomming()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && !string.IsNullOrEmpty(t.IncomingTransNo)).ToList();
        }
        public List<PECC2Documents> GetAllOverdueDocument()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && !string.IsNullOrEmpty(t.IncomingTransNo)
                && string.IsNullOrEmpty(t.OutgoingTransNo)).ToList();
        }
        public List<PECC2Documents> GetAllOutGoing()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && t.IsLeaf.GetValueOrDefault()
                && !string.IsNullOrEmpty(t.OutgoingTransNo)).ToList();
        }
        //public List<PECC2Documents> GetByPlant(int PlantID)
        //{
        //    return this.repo.GetByPlant(PlantID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByArea(int AreaID)
        //{
        //    return this.repo.GetByArea(AreaID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByUnit(int codeID)
        //{
        //    return this.repo.GetByUnit(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}

        //public List<PECC2Documents> GetByDocumentType(int codeID)
        //{
        //    return this.repo.GetByDocumentType(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByMaterial(int codeID)
        //{
        //    return this.repo.GetByMaterial(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByWork(int codeID)
        //{
        //    return this.repo.GetByWork(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByDrawing(int codeID)
        //{
        //    return this.repo.GetByDrawing(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList(); 
        //}
        //public List<PECC2Documents> GetByOriginator(int OriginatorID)
        //{
        //    return this.repo.GetByOriginator(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByOriginatingOrganization(int OriginatorID)
        //{
        //    return this.repo.GetByOriginatingOrganization(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByReceivingOrganization(int OriginatorID)
        //{
        //    return this.repo.GetByReceivingOrganization(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault()).ToList();
        //}
        //public List<PECC2Documents> GetByProjectCode(int projectID, bool isGetAll)
        //{
        //    return this.repo.GetByProjectCode(projectID).Where(t=> !t.IsDelete.GetValueOrDefault()
        //        && (isGetAll || t.IsLeaf.GetValueOrDefault())).ToList();
        //}
        //public List<PECC2Documents> GetByDepartment(string DepartmentCode)
        //{
        //    return this.repo.GetByDepartment(DepartmentCode).Where(t=> !t.IsDelete.GetValueOrDefault()
        //        && t.IsLeaf.GetValueOrDefault() ).ToList();
        //}
        public bool IsExistByDocNo(string docNo)
        {
            return this.repo.GetAll().Any(t => t.IsLeaf.GetValueOrDefault() && !t.IsDelete.GetValueOrDefault() && t.DocNo == docNo);
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
        public List<PECC2Documents> GetAllRelatedDocument(Guid docId)
        {
            return this.repo.GetAll().Where(t => (t.ID == docId || t.ParentId == docId) && !t.IsDelete.GetValueOrDefault()).ToList();
        }

        public List<PECC2Documents> GetAllRevDoc(Guid parentId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && (t.ID == parentId || t.ParentId == parentId)).OrderByDescending(t => t.CreatedDate).ToList();
        }



        public List<PECC2Documents> SearchDocument(int projectId, int disciplineId, string docNo, string docTitle, string searchFullFields, bool isGetAllRevision)
        {
            return this.repo.GetAll().Where(
                t =>
                  !t.IsDelete.GetValueOrDefault()
                && (projectId == t.ProjectId.GetValueOrDefault() || projectId == 0)
                && (disciplineId == 0 || t.DisciplineId.GetValueOrDefault() == disciplineId)
                && (string.IsNullOrEmpty(docNo) || t.DocNo.ToLower().Contains(docNo.ToLower()))
                && (string.IsNullOrEmpty(docTitle) || t.DocTitle.ToLower().Contains(docTitle.ToLower()))
                ).OrderBy(t => t.DocNo).ToList();
        }

        public List<PECC2Documents> SearchDocument(string searchAll, string docNo, string title, int docTypeId, int projectId, int disciplineId, int revisonSchemaid, int engineerId)
        {
            return this.repo.GetAll().Where(
                t =>
                t.IsLeaf.GetValueOrDefault()
                && !t.IsDelete.GetValueOrDefault()
               
                && (string.IsNullOrEmpty(docNo) || t.DocNo.ToLower().Contains(docNo.ToLower()))
                && (string.IsNullOrEmpty(title) || t.DocTitle.ToLower().Contains(title.ToLower()))
                && (disciplineId == 0 || (t.DisciplineId != null && t.DisciplineId == disciplineId))
                && (projectId == 0 || (t.ProjectId != null && t.ProjectId == projectId))
                && (docTypeId == 0 || (t.DocTypeId != null && t.DocTypeId == docTypeId))
                && (revisonSchemaid == 0 || (t.RevisionSchemaId != null && t.RevisionSchemaId == revisonSchemaid))
               

                && (string.IsNullOrEmpty(searchAll)
                    || (!string.IsNullOrEmpty(t.DocNo) && t.DocNo.ToLower().Contains(searchAll.ToLower()))
                    || (!string.IsNullOrEmpty(t.DocTitle) && t.DocTitle.ToLower().Contains(searchAll.ToLower()))
                    || (!string.IsNullOrEmpty(t.ProjectName) && t.ProjectName.ToLower().Contains(searchAll.ToLower()))
                    || (!string.IsNullOrEmpty(t.DisciplineCode) && t.DisciplineCode.ToLower().Contains(searchAll.ToLower()))
                    )).OrderBy(t => t.DocNo).ToList();
        }

        public List<PECC2Documents> GetAllDocList(List<Guid> listdoc)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.IsLeaf.GetValueOrDefault() && listdoc.Contains(t.ID)).ToList();
        }

        public List<PECC2Documents> GetSpecialDocList(List<Guid> listdoc)
        {
            return this.repo.GetAll().Where(t => listdoc.Contains(t.ID)).ToList();
        }

        //public List<PECC2Documents> GetAllByMaster(Guid MasterId)
        //{
        //    return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.DocumentMasterId==MasterId).OrderBy(t=> t.CreatedDate).ToList();
        //}
        //public List<PECC2Documents> GetAllByMaster(List<Guid> MasterId)
        //{
        //    return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && MasterId.Contains(t.DocumentMasterId.GetValueOrDefault())).ToList();
        //}
        public bool IsExist(string documentNumber, Guid docId)
        {
            var docObj = this.GetById(docId);
            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.DocNo == documentNumber
                &&  (t.ID != docObj.ID
                        && t.ID != docObj.ParentId));
        }

        public bool IsExist(string documentNumber)
        {
            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.DocNo == documentNumber);
        }


        public bool IsExist(string documentNumber,string revname)
        {

            return this.repo.GetAll().Any(t =>
                !t.IsDelete.GetValueOrDefault()
                && t.DocNo == documentNumber
                && t.Revision == revname.Trim());
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<PECC2Documents> GetAllLatest()
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
        public List<PECC2Documents> GetAll()
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
        public PECC2Documents GetById(Guid id)
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
        public Guid? Insert(PECC2Documents bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(PECC2Documents bo)
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
        public bool Delete(PECC2Documents bo)
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
