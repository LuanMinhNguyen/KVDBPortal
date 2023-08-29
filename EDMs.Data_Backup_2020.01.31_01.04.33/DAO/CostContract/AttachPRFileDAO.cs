// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AttachPRFileDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.CostContract
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class AttachPRFileDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AttachPRFileDAO"/> class.
        /// </summary>
        public AttachPRFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<AttachPRFile> GetIQueryable()
        {
            return this.EDMsDataContext.AttachPRFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AttachPRFile> GetAll()
        {
            return this.EDMsDataContext.AttachPRFiles.ToList();
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
        public AttachPRFile GetById(int id)
        {
            return this.EDMsDataContext.AttachPRFiles.FirstOrDefault(ob => ob.ID == id);
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
        public List<AttachPRFile> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.AttachPRFiles.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(AttachPRFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToAttachPRFiles(ob);
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
        public bool Update(AttachPRFile src)
        {
            try
            {
                AttachPRFile des = (from rs in this.EDMsDataContext.AttachPRFiles
                                where rs.ID == src.ID
                                select rs).First();

                des.Filename = src.Filename;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
                des.IsDefault = des.IsDefault;

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
        public bool Delete(AttachPRFile src)
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
