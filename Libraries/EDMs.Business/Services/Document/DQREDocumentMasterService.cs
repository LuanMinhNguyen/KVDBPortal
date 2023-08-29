

namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.DAO.Scope;
    using EDMs.Data.Entities;
    public class DQREDocumentMasterService
    { /// <summary>
      /// The repo.
      /// </summary>
        private readonly DQREDocumentMasterDAO repo;


        /// <summary>
        /// Initializes a new instance of the <see cref="DQREDocumentMasterService"/> class.
        /// </summary>
        public DQREDocumentMasterService()
        {
            this.repo = new DQREDocumentMasterDAO();
        }

        #region Get (Advances)

        public DQREDocumentMaster GetOneByDocNo(string docNo)
        {
            return this.repo.GetAll().FirstOrDefault(t => !t.IsDelete.GetValueOrDefault()
                    && t.SystemDocumentNo.Trim() == docNo.Trim());
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
        public List<DQREDocumentMaster> GetByDiscipline(int codeID)
        {
            return this.repo.GetByDiscipline(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }

        public List<DQREDocumentMaster> GetByPlant(int PlantID)
        {
            return this.repo.GetByPlant(PlantID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByArea(int AreaID)
        {
            return this.repo.GetByArea(AreaID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByUnit(int codeID)
        {
            return this.repo.GetByUnit(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }

        public List<DQREDocumentMaster> GetByDocumentType(int codeID)
        {
            return this.repo.GetByDocumentType(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByMaterial(int codeID)
        {
            return this.repo.GetByMaterial(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByWork(int codeID)
        {
            return this.repo.GetByWork(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByDrawing(int codeID)
        {
            return this.repo.GetByDrawing(codeID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByOriginator(int OriginatorID)
        {
            return this.repo.GetByOriginator(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByOriginatingOrganization(int OriginatorID)
        {
            return this.repo.GetByOriginatingOrganization(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public List<DQREDocumentMaster> GetByReceivingOrganization(int OriginatorID)
        {
            return this.repo.GetByReceivingOrganization(OriginatorID).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
     
        public List<DQREDocumentMaster> GetByDepartment(string DepartmentCode)
        {
            return this.repo.GetByDepartment(DepartmentCode).Where(t => !t.IsDelete.GetValueOrDefault()
                ).ToList();
        }
        public bool IsExistByDocNo(string docNo)
        {
            return this.repo.GetAll().Any(t =>  !t.IsDelete.GetValueOrDefault() && t.SystemDocumentNo == docNo);
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
      

        
       

       

        public List<DQREDocumentMaster> GetAllDocList(List<Guid> listdoc)
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()  && listdoc.Contains(t.ID)).ToList();
        }
    
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DQREDocumentMaster> GetAll()
        {
            return this.repo.GetAll().Where(t => !t.IsDelete.GetValueOrDefault()).ToList();
        }


        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DQREDocumentMaster GetById(Guid id)
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
        public Guid? Insert(DQREDocumentMaster bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DQREDocumentMaster bo)
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
        public bool Delete(DQREDocumentMaster bo)
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
