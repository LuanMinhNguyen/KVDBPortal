using System;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class NCR_SIAddPictureDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NCR_SIAddPictureDAO"/> class.
        /// </summary>
        public NCR_SIAddPictureDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<NCR_SIAddPicture> GetIQueryable()
        {
            return this.EDMsDataContext.NCR_SIAddPicture;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<NCR_SIAddPicture> GetAll()
        {
            return this.EDMsDataContext.NCR_SIAddPicture.ToList();
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
        public NCR_SIAddPicture GetById(Guid id)
        {
            return this.EDMsDataContext.NCR_SIAddPicture.FirstOrDefault(ob => ob.ID == id);
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
        public List<NCR_SIAddPicture> GetNCRSIID(Guid tranId, int type)
        {
            return this.EDMsDataContext.NCR_SIAddPicture.Where(t => t.NCR_SIId == tranId && t.TypeId==type).ToList();
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
        public Guid? Insert(NCR_SIAddPicture ob)
        {
            try
            {
                this.EDMsDataContext.AddToNCR_SIAddPicture(ob);
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
        public bool Update(NCR_SIAddPicture src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.NCR_SIAddPicture
                           where rs.ID == src.ID
                           select rs).First();

                des.Description = src.Description;
                des.FilePath = src.FilePath;
                des.TypeId = src.TypeId;
                des.NCR_SIId = src.NCR_SIId;
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
        public bool Delete(NCR_SIAddPicture src)
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
