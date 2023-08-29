// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachFilesPackageDAO.cs" company="">
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
    public class AttachFilesPackageDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachFilesPackageDAO"/> class.
        /// </summary>
        public AttachFilesPackageDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<AttachFilesPackage> GetIQueryable()
        {
            return this.EDMsDataContext.AttachFilesPackages;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AttachFilesPackage> GetAll()
        {
            return this.EDMsDataContext.AttachFilesPackages.ToList();
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
        public AttachFilesPackage GetById(int id)
        {
            return this.EDMsDataContext.AttachFilesPackages.FirstOrDefault(ob => ob.ID == id);
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
        public List<AttachFilesPackage> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.AttachFilesPackages.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(AttachFilesPackage ob)
        {
            try
            {
                this.EDMsDataContext.AddToAttachFilesPackages(ob);
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
        public bool Update(AttachFilesPackage src)
        {
            try
            {
                AttachFilesPackage des = (from rs in this.EDMsDataContext.AttachFilesPackages
                                where rs.ID == src.ID
                                select rs).First();

                des.FileName = src.FileName;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
                des.FileSize = src.FileSize;
                des.AttachType = src.AttachType;
                des.AttachTypeName = src.AttachTypeName;
                des.CmtResFrom = src.CmtResFrom;
                des.CmtResTo = src.CmtResTo;
                des.Description = src.Description;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.FilePathEncrypt = src.FilePathEncrypt;
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
        public bool Delete(AttachFilesPackage src)
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
