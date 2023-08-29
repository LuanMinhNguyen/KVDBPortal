

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using EDMs.Data.Entities;

  public  class DQREDocumentAttachFileDAO: BaseDAO
    {/// <summary>
     /// Initializes a new instance of the <see cref="DQREDocumentAttachFileDAO"/> class.
     /// </summary>
        public DQREDocumentAttachFileDAO() : base() { }

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DQREDocumentAttachFile> GetIQueryable()
        {
            return this.EDMsDataContext.DQREDocumentAttachFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DQREDocumentAttachFile> GetAll()
        {
            return this.EDMsDataContext.DQREDocumentAttachFiles.ToList();
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
        public DQREDocumentAttachFile GetById(Guid id)
        {
            return this.EDMsDataContext.DQREDocumentAttachFiles.FirstOrDefault(ob => ob.ID == id);
        }



   

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
        public Guid? Insert(DQREDocumentAttachFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToDQREDocumentAttachFiles(ob);
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
        public bool Update(DQREDocumentAttachFile src)
        {
            try
            {
                DQREDocumentAttachFile des = (from rs in this.EDMsDataContext.DQREDocumentAttachFiles
                                          where rs.ID == src.ID
                                          select rs).First();

                des.FileName = src.FileName;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
                des.FileSize = src.FileSize;
                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;
                des.IsOnlyMarkupPage = des.IsOnlyMarkupPage;
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
        public bool Delete(DQREDocumentAttachFile src)
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
