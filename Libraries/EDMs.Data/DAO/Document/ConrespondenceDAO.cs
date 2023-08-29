namespace EDMs.Data.DAO.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO;
    using EDMs.Data.Entities;
  public  class ConrespondenceDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConrespondenceDAO"/> class.
        /// </summary>
        public ConrespondenceDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Conrespondence> GetIQueryable()
        {
            return this.EDMsDataContext.Conrespondences;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetAll()
        {
            return this.EDMsDataContext.Conrespondences.ToList();
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
        public Conrespondence GetById(int id)
        {
            return this.EDMsDataContext.Conrespondences.FirstOrDefault(ob => ob.ID == id);
        }

        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get all by folder.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>. Conrespondence
        /// </returns>
        public List<Conrespondence> GetAllByFolder(int folderId)
        {
            return this.EDMsDataContext.Conrespondences.Where(t => t.FolderID == folderId  && t.IsDelete == false && t.IsLeaf==true).OrderByDescending(t => t.ID).ToList();
        }

        public List<Conrespondence> GetAllByFolder(List<int> folderIds)
        {
            return this.EDMsDataContext.Conrespondences.ToArray().Where(t => folderIds.Contains(t.FolderID.GetValueOrDefault())  && t.IsDelete == false && t.IsLeaf == true).OrderByDescending(t => t.ID).ToList();

        }

        public int GetItemCount()
        {
            return this.EDMsDataContext.Conrespondences.Count(t => t.IsDelete == false && t.IsLeaf == true);
        }
   

        /// <summary>
        /// The get all doc version.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetAllDocVersion(int docId)
        {
            return this.EDMsDataContext.Conrespondences.Where(t => (t.ID == docId || t.ParentID == docId) && t.IsDelete == false && t.IsLeaf == true).ToList();
        }

        /// <summary>
        /// The quick search.
        /// </summary>
        /// <param name="keyword">
        /// The keyword.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> QuickSearch(string keyword, int folId)
        {
            return this.EDMsDataContext.Conrespondences.Where(t => (t.DocumentNumber.ToLower().Contains(keyword.ToLower()) || t.Title.ToLower().Contains(keyword.ToLower()) || t.FileName.ToLower().Contains(keyword.ToLower())) &&
                t.FolderID == folId && t.IsLeaf == true && t.IsDelete == false).OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        /// The get all relate Conrespondence.
        /// </summary>
        /// <param name="docId">
        /// The doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetAllRelateConrespondence(int docId)
        {
            return this.EDMsDataContext.Conrespondences.Where(t => t.ID == docId || t.ParentID == docId).ToList();
        }

 


        /// <summary>
        /// The is Conrespondence exist.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="ConrespondenceName">
        /// The Conrespondence name.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsConrespondenceExist(int folderId, string ConrespondenceName)
        {
            return
                this.EDMsDataContext.Conrespondences.Any(
                    t =>
                    t.IsDelete == false &&
                    t.FolderID == folderId &&
                    t.FileName == ConrespondenceName);
        }

        /// <summary>
        /// The is Conrespondence exist.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="fileName">
        /// The file Name.
        /// </param>
        /// <param name="docId"> </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool IsConrespondenceExistUpdate(int folderId, string docName, int docId)
        {
            return this.EDMsDataContext.Conrespondences.Any(t => t.FolderID == folderId && t.FileName == docName  && t.ID != docId && t.IsDelete==false);
        }

        /// <summary>
        /// The get specific Conrespondence.
        /// </summary>
        /// <param name="folderId">
        /// The folder id.
        /// </param>
        /// <param name="ConrespondenceName">
        /// The Conrespondence name.
        /// </param>
        /// <returns>
        /// The <see cref="Conrespondence"/>.
        /// </returns>
        public Conrespondence GetSpecificConrespondence(int folderId, string ConrespondenceNumber)
        {
            var docList = this.EDMsDataContext.Conrespondences.FirstOrDefault(t => t.FolderID == folderId && t.FileName == ConrespondenceNumber && t.IsDelete==false);
            return docList;
        }

        /// <summary>
        /// The get specific Conrespondence.
        /// </summary>
        /// <param name="listDocId">
        /// The list doc id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Conrespondence> GetSpecificConrespondence(List<int> listDocId)
        {
            return this.EDMsDataContext.Conrespondences.ToArray().Where(t => listDocId.Contains(t.ID)).ToList();
        }

        /// <summary>
        /// The get specific Conrespondence.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <param name="dirName">
        /// The dir name.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public Conrespondence GetSpecificConrespondence(string fileName, string dirName)
        {
            return this.EDMsDataContext.Conrespondences.FirstOrDefault(t => t.DirName == dirName && t.FileName == fileName && t.IsDelete == false);
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
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(Conrespondence ob)
        {
            try
            {
                this.EDMsDataContext.AddToConrespondences(ob);
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
        public bool Update(Conrespondence src)
        {
            try
            {
                Conrespondence des = (from rs in this.EDMsDataContext.Conrespondences
                                where rs.ID == src.ID
                                select rs).First();

                des.FileName = src.FileName;
                des.DocumentNumber = src.DocumentNumber;
                des.Title = src.Title;
                des.FromID = src.FromID;
                des.FromName = src.FromName;
                des.ToID = src.ToID;
                des.ToName = src.ToName;
                des.DocumentTypeID = src.DocumentTypeID;
                des.DocumentTypeName = src.DocumentTypeName;
                des.DisciplineID = src.DisciplineID;
                des.DisciplineName = src.DisciplineName;
                des.IssueDate = src.IssueDate;
                des.AnswerRequestDate = src.AnswerRequestDate;
                des.ReferenceDocs = src.ReferenceDocs;
                des.Remark = src.Remark;
                des.LeaderId = src.LeaderId;
                des.UserInforId = src.UserInforId;
                des.UserInforName = src.UserInforName;
                des.Leader = src.Leader;
                des.IsLeaf = src.IsLeaf;
                des.IsDelete = src.IsDelete;
                des.ParentID = src.ParentID;
                des.FolderID = src.FolderID;
                des.DirName = src.DirName;
                des.FilePath = src.FilePath;
                des.FileExtension = src.FileExtension;
                des.FileExtensionIcon = src.FileExtensionIcon;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.Reply = src.Reply;

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
        public bool Delete(Conrespondence src)
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
