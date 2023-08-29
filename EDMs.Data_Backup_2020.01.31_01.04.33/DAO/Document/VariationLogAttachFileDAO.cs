namespace EDMs.Data.DAO.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
  public  class VariationLogAttachFileDAO: BaseDAO
    { /// <summary>
      /// Initializes a new instance of the <see cref="VariationLogAttachFileDAO"/> class.
      /// </summary>
        public VariationLogAttachFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<VariationLogAttachFile> GetIQueryable()
        {
            return this.EDMsDataContext.VariationLogAttachFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<VariationLogAttachFile> GetAll()
        {
            return this.EDMsDataContext.VariationLogAttachFiles.ToList();
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
        public VariationLogAttachFile GetById(Guid id)
        {
            return this.EDMsDataContext.VariationLogAttachFiles.FirstOrDefault(ob => ob.ID == id);
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
        public List<VariationLogAttachFile> GetSpecific(Guid tranId)
        {
            return this.EDMsDataContext.VariationLogAttachFiles.Where(t => t.ID == tranId).ToList();
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
        public Guid? Insert(VariationLogAttachFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToVariationLogAttachFiles(ob);
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
        public bool Update(VariationLogAttachFile src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.VariationLogAttachFiles
                           where rs.ID == src.ID
                           select rs).First();

                des.FileName = src.FileName;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;
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
        public bool Delete(VariationLogAttachFile src)
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
