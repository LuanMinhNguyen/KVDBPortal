// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentNewService.cs" company="">
//   
// </copyright>
// <summary>
//   The category service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.Entities;

    /// <summary>
    /// The category service.
    /// </summary>
    public class DocumentNewService
    {
        /// <summary>
        /// The repo.
        /// </summary>
        private readonly DocumentNewDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNewService"/> class.
        /// </summary>
        public DocumentNewService()
        {
            this.repo = new DocumentNewDAO();
        }

        #region Get (Advances)

        /// <summary>
        /// The get specific.
        /// </summary>
        /// <param name="tranId">
        /// The tran id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentNew> GetSpecific(int tranId)
        {
            return this.repo.GetSpecific(tranId);
        }

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="DocumentNew"/>.
        /// </returns>
        public DocumentNew GetByName(string name, int categoryId)
        {
            return this.repo.GetAllCurrentDoc().FirstOrDefault(t => t.Name == name && t.CategoryId == categoryId);
        }


        public List<DocumentNew> GetAllByTagType(string tagtypeId)
        {
            return
                this.repo.GetAllCurrentDoc().Where(
                    t =>
                        !string.IsNullOrEmpty(t.TagTypeId) &&
                        t.TagTypeId.Split(',').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim()).Contains(
                        tagtypeId)).ToList();
        }

        /// <summary>
        /// The get all rev doc.
        /// </summary>
        /// <param name="parentId">
        /// The parent id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentNew> GetAllRevDoc(int parentId)
        {
            return this.repo.GetAll().Where(t => (t.ID == parentId || t.ParentId == parentId) && t.RevId != 0).OrderByDescending(t => t.RevId).ToList();
        }
        #endregion

        #region GET (Basic)
        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DocumentNew> GetAllCurrentDoc(int categoryId)
        {
            return this.repo.GetAllCurrentDoc().Where(t => t.CategoryId == categoryId).ToList();
        }

        /// <summary>
        /// Get All Categories
        /// </summary>
        /// <returns>
        /// The category
        /// </returns>
        public List<DocumentNew> GetAllCurrentDoc()
        {
            return this.repo.GetAllCurrentDoc();
        }

        ////public List<DocumentNew> GetAllCurrentDoc(int categoryId, int startingRecordNumber, int pageSize)
        ////{
        ////    return this.repo.GetAllCurrentDoc().Where(t => t.CategoryId == categoryId).OrderByDescending(t => t.ID).Skip(startingRecordNumber).Take(pageSize).ToList();
        ////}

        public List<DocumentNew> GetAllCurrentDoc(int categoryId, int roleId, int userId)
        {
            return this.repo.GetAllCurrentDoc().Where(t =>
                (t.CreatedBy == userId || 
                t.IsPublish.GetValueOrDefault() || 
                (!t.IsPrivate.GetValueOrDefault() && !t.IsPublish.GetValueOrDefault() && t.RoleId == roleId)) && 
                t.CategoryId == categoryId).ToList();
        }

        public List<DocumentNew> GetAllRelateDocument(int docId)
        {
            return this.repo.GetAllRelateDocument(docId);
        }


        public List<DocumentNew> SearchDocument(int categoryId, int roleId, string name, string description, int revId, string vendorName, string drawingNumber, int year, int plantId, int systemId, int disciplineId, int documentTypeId, string tagTypeId, int projectId, int blockId, int fieldId, int platformId, int wellId, DateTime? startDate, DateTime? endDate, int numberOfWork, string tagNo, string tagDes, string manufacture, string serialNo, string modelNo, string assetNo, string tableOfContent, DateTime? publishDate, int fromId, int toId, string signer, string other, int rigId, string kindOfRepair, string searchFullFields)
        {
            return this.repo.GetAll().Where(
                t =>
                t.IsLeaf == true
                && t.CategoryId == categoryId
                && (roleId == 0 || t.RoleId == roleId)
                && (string.IsNullOrEmpty(name) || t.Name.ToLower().Contains(name.ToLower()))
                && (string.IsNullOrEmpty(description) || t.Description.ToLower().Contains(description.ToLower()))
                && (revId == 0 || t.RevId == revId)
                && (string.IsNullOrEmpty(vendorName) || t.VendorName.ToLower().Contains(vendorName.ToLower()))
                && (string.IsNullOrEmpty(drawingNumber) || t.DrawingNumber.ToLower().Contains(drawingNumber.ToLower()))
                && (year == 0 || t.Year == year)
                && (plantId == 0 || t.PlantId == plantId)
                && (systemId == 0 || t.SystemId == systemId)
                && (disciplineId == 0 || t.DisciplineId == disciplineId)
                && (documentTypeId == 0 || t.DocumentTypeId == documentTypeId)
                && (string.IsNullOrEmpty(tagTypeId) || (!string.IsNullOrEmpty(t.TagTypeId) && t.TagTypeId.Contains(tagTypeId)))
                && (projectId == 0 || t.ProjectId == projectId)
                && (blockId == 0 || t.BlockId == blockId)
                && (fieldId == 0 || t.FieldId == fieldId)
                && (platformId == 0 || t.PlatformId == platformId)
                && (wellId == 0 || t.WellId == wellId)
                && (startDate == null || t.StartDate == startDate)
                && (endDate == null || t.EndDate == endDate)
                && (numberOfWork == 0 || t.NumberOfWork == numberOfWork)
                && (string.IsNullOrEmpty(tagNo) || t.TagNo.ToLower().Contains(tagNo.ToLower()))
                && (string.IsNullOrEmpty(tagDes) || t.TagDes.ToLower().Contains(tagDes.ToLower()))
                && (string.IsNullOrEmpty(manufacture) || t.Manufacturers.ToLower().Contains(manufacture.ToLower()))
                && (string.IsNullOrEmpty(serialNo) || t.SerialNo.ToLower().Contains(serialNo.ToLower()))
                && (string.IsNullOrEmpty(modelNo) || t.ModelNo.ToLower().Contains(modelNo.ToLower()))
                && (string.IsNullOrEmpty(assetNo) || t.AssetNo.ToLower().Contains(assetNo.ToLower()))
                && (string.IsNullOrEmpty(tableOfContent) || t.TableOfContents.ToLower().Contains(tableOfContent.ToLower()))
                && (publishDate == null || t.PublishDate == publishDate)
                && (fromId == 0 || t.FromId == fromId)
                && (toId == 0 || t.ToId == toId)
                && (rigId == 0 || t.RIGId == rigId)
                && (string.IsNullOrEmpty(signer) || t.Signer.ToLower().Contains(signer.ToLower()))
                && (string.IsNullOrEmpty(other) || t.Other.ToLower().Contains(other.ToLower()))
                && (string.IsNullOrEmpty(kindOfRepair) || t.KindOfRepair.ToLower().Contains(kindOfRepair.ToLower()))
                && (string.IsNullOrEmpty(searchFullFields)
                    || (!string.IsNullOrEmpty(t.Name) && t.Name.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.Description) && t.Description.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.RevName) && t.RevName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.VendorName) && t.VendorName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.DrawingNumber) && t.DrawingNumber.ToLower().Contains(searchFullFields.ToLower()))
                    || t.Year.ToString().ToLower().Contains(searchFullFields.ToLower())
                    || (!string.IsNullOrEmpty(t.PlantName) && t.PlantName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.SystemName) && t.SystemName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.DisciplineName) && t.DisciplineName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.DocumentTypeName) && t.DocumentTypeName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.TagTypeName) && t.TagTypeName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.ProjectName) && t.ProjectName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.BlockName) && t.BlockName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.FieldName) && t.FieldName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.PlatformName) && t.PlatformName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.WellName) && t.WellName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.TagNo) && t.TagNo.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.TagDes) && t.TagDes.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.Manufacturers) && t.Manufacturers.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.SerialNo) && t.SerialNo.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.ModelNo) && t.ModelNo.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.AssetNo) && t.AssetNo.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.TableOfContents) && t.TableOfContents.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.FromName) && t.FromName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.ToName) && t.ToName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.Signer) && t.Signer.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.RIGName) && t.RIGName.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.KindOfRepair) && t.KindOfRepair.ToLower().Contains(searchFullFields.ToLower()))
                    || (!string.IsNullOrEmpty(t.Other) && t.Other.ToLower().Contains(searchFullFields.ToLower())))).ToList();
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
        public List<DocumentNew> GetAllByOwner(int createdBy)
        {
            return this.repo.GetAllByOwner(createdBy);
        }

        /// <summary>
        /// Get Resource By ID
        /// </summary>
        /// <param name="id">
        /// ID of category
        /// </param>
        /// <returns>
        /// The category</returns>
        public DocumentNew GetById(int id)
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
        public int? Insert(DocumentNew bo)
        {
            return this.repo.Insert(bo);
        }

        /// <summary>
        /// Update Resource
        /// </summary>
        /// <param name="bo"></param>
        /// <returns></returns>
        public bool Update(DocumentNew bo)
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
        public bool Delete(DocumentNew bo)
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
