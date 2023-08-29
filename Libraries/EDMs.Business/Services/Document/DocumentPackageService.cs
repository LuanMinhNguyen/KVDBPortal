namespace EDMs.Business.Services.Library
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
    public class DocumentPackageService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DocumentPackageDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPackageService"/> class.
        /// </summary>
        public DocumentPackageService()
        {
            this.repo = new DocumentPackageDAO();
        }

        #region Get (Advances)

        public DocumentPackage GetOneByDocNo(string docNo, string revName, int Project)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault() 
                && t.DocNo.Trim().Replace(" ", string.Empty) == docNo.Trim().Replace(" ", string.Empty) 
                && t.RevisionName == revName 
                && t.ProjectId==Project);
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
        public List<DocumentPackage> GetAllByPackage(int packageId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() 
                && t.IsLeaf.GetValueOrDefault() 
                && t.PackageId == packageId).ToList();
        }

        public List<DocumentPackage> GetAllByWorkgroup(int workgroupId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.IsLeaf.GetValueOrDefault() && t.WorkgroupId == workgroupId).ToList();
        }

        public List<DocumentPackage> GetAllByDiscipline(int disciplineID)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() 
                && t.IsLeaf.GetValueOrDefault() 
                && t.DisciplineId == disciplineID).ToList();
        }

        public List<DocumentPackage> GetAllByDiscipline(int disciplineID, bool isGetAll)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()
                && (isGetAll || t.IsLeaf.GetValueOrDefault())
                && t.DisciplineId == disciplineID).ToList();
        }

        public List<DocumentPackage> GetAllByWorkgroupInPermission(List<int> listworkgroupId)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.IsLeaf.GetValueOrDefault() && listworkgroupId.Contains(t.WorkgroupId.GetValueOrDefault())).ToList();
        }

        public List<DocumentPackage> GetAllByDisciplineInPermission(List<int> listDisciplineId, bool isGetAll)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() 
                && (isGetAll || t.IsLeaf.GetValueOrDefault())
                && listDisciplineId.Contains(t.DisciplineId.GetValueOrDefault())).ToList();
        }

        public List<DocumentPackage> GetAllByProject(int projectId , bool isGetAll)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() 
                && (isGetAll || t.IsLeaf.GetValueOrDefault())
                && t.ProjectId == projectId).ToList();
        }

        public List<DocumentPackage> GetAllEMDRByWorkgroup(int workgroupId, bool isGetAll)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() && t.IsLeaf.GetValueOrDefault() && t.WorkgroupId == workgroupId && (isGetAll || t.IsEMDR.GetValueOrDefault())).ToList();
        }

        public List<DocumentPackage> GetAllEMDRByDiscipline(int disciplineId, bool isGetAll)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault() 
                    && t.IsLeaf.GetValueOrDefault() 
                    && t.DisciplineId == disciplineId 
                    && (isGetAll || t.IsEMDR.GetValueOrDefault())).ToList();
        }

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
        public List<DocumentPackage> GetAllRelatedDocument(int docId)
        {
            return this.repo.GetAll().Where(t => (t.ID == docId || t.ParentId == docId) && !t.IsDelete.GetValueOrDefault()).ToList();
        }

        public List<DocumentPackage> GetAllRevDoc(int parentId)
        {
            return this.repo.GetAll().Where(t => (t.ID == parentId || t.ParentId == parentId) && t.RevisionId != 0).OrderByDescending(t => t.RevisionId).ToList();
        }

        public List<DocumentPackage> SearchDocument(int projectId, int disciplineId, string docNo, string docTitle, string searchFullFields, bool isGetAllRevision)
        {
            return this.repo.GetAll().Where(
                t =>
                (isGetAllRevision || t.IsLeaf.GetValueOrDefault())
                && !t.IsDelete.GetValueOrDefault()
                //&& t.IsEMDR.GetValueOrDefault()
                && (disciplineId == 0 ||t.DisciplineId == disciplineId)
                && (projectId == 0 || t.ProjectId == projectId)
                && (string.IsNullOrEmpty(docNo) || t.DocNo.ToLower().Contains(docNo.ToLower()))
                && (string.IsNullOrEmpty(docTitle) || t.DocTitle.ToLower().Contains(docTitle.ToLower()))
                && (string.IsNullOrEmpty(searchFullFields)
                    || (!string.IsNullOrEmpty(t.DocNo) && t.DocNo.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.DocTitle) && t.DocTitle.ToLower().Contains(searchFullFields.ToLower())))).OrderBy(t => t.DocNo).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DocumentPackage> GetAll()
        {
            return this.repo.GetAll().ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DocumentPackage GetById(int id)
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
        public int? Insert(DocumentPackage bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DocumentPackage bo)
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
        public bool Delete(DocumentPackage bo)
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
        public bool Delete(int id)
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
