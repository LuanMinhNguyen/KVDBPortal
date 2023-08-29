// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentNewDAO.cs" company="">
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

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class DocumentNewDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentNewDAO"/> class.
        /// </summary>
        public DocumentNewDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DocumentNew> GetIQueryable()
        {
            return this.EDMsDataContext.DocumentNews;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentNew> GetAllCurrentDoc()
        {
            return this.EDMsDataContext.DocumentNews.Where(t => t.IsLeaf == true).ToList();
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentNew> GetAll()
        {
            return this.EDMsDataContext.DocumentNews.ToList();
        }


        /// <summary>
        /// The get all relate document.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentNew> GetAllRelateDocument(int docId)
        {
            return this.EDMsDataContext.DocumentNews.Where(t => t.ID == docId || t.ParentId == docId).ToList();
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
            return this.EDMsDataContext.DocumentNews.Where(t => t.CreatedBy == createdBy).ToList();
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
        public DocumentNew GetById(int id)
        {
            return this.EDMsDataContext.DocumentNews.FirstOrDefault(ob => ob.ID == id);
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
        public List<DocumentNew> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.DocumentNews.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(DocumentNew ob)
        {
            try
            {
                this.EDMsDataContext.AddToDocumentNews(ob);
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
        public bool Update(DocumentNew src)
        {
            try
            {
                DocumentNew des = (from rs in this.EDMsDataContext.DocumentNews
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.RevId = src.RevId;
                des.RevName = src.RevName;
                des.VendorName = src.VendorName;
                des.DrawingNumber = src.DrawingNumber;
                des.Year = src.Year;
                des.PlantId = src.PlantId;
                des.PlantName = src.PlantName;
                des.SystemId = src.SystemId;
                des.SystemName = src.SystemName;
                des.DisciplineId = src.DisciplineId;
                des.DisciplineName = src.DisciplineName;
                des.DocumentTypeId = src.DocumentTypeId;
                des.DocumentTypeName = src.DocumentTypeName;
                des.TagTypeId = src.TagTypeId;
                des.TagTypeName = src.TagTypeName;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.BlockId = src.BlockId;
                des.BlockName = src.BlockName;
                des.FieldId = src.FieldId;
                des.FieldName = src.FieldName;
                des.PlatformId = src.PlatformId;
                des.PlatformName = src.PlatformName;
                des.WellId = src.WellId;
                des.WellName = src.WellName;
                des.StartDate = src.StartDate;
                des.EndDate = src.EndDate;
                des.NumberOfWork = src.NumberOfWork;
                des.TagNo = src.TagNo;
                des.TagDes = src.TagDes;
                des.Manufacturers = src.Manufacturers;
                des.SerialNo = src.SerialNo;
                des.ModelNo = src.ModelNo;
                des.AssetNo = src.AssetNo;
                des.TableOfContents = src.TableOfContents;
                des.PublishDate = src.PublishDate;
                des.FromId = src.FromId;
                des.FromName = src.FromName;
                des.ToId = src.ToId;
                des.ToName = src.ToName;
                des.Signer = src.Signer;
                des.RoleId = src.RoleId;
                des.IsPublish = src.IsPublish;
                des.IsPrivate = src.IsPrivate;
                des.IsLeaf = src.IsLeaf;

                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

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
        public bool Delete(DocumentNew src)
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
