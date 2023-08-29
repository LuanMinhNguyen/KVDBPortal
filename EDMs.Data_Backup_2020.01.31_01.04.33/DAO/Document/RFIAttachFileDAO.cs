using System;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
   public  class RFIAttachFileDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RFIAttachFileDAO"/> class.
        /// </summary>
        public RFIAttachFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<RFIAttachFile> GetIQueryable()
        {
            return this.EDMsDataContext.RFIAttachFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<RFIAttachFile> GetAll()
        {
            return this.EDMsDataContext.RFIAttachFiles.ToList();
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
        public RFIAttachFile GetById(Guid id)
        {
            return this.EDMsDataContext.RFIAttachFiles.FirstOrDefault(ob => ob.ID == id);
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<RFIAttachFile> GetAllRFI( Guid RFIID)
        {
            return this.EDMsDataContext.RFIAttachFiles.Where(t=> t.RFIId==RFIID).ToList();
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
        public Guid? Insert(RFIAttachFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToRFIAttachFiles(ob);
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
        public bool Update(RFIAttachFile src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.RFIAttachFiles
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
        public bool Delete(RFIAttachFile src)
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
