// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PECC2ObjectNumberingDAO.cs" company="">
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
    public class PECC2ObjectNumberingDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2ObjectNumberingDAO"/> class.
        /// </summary>
        public PECC2ObjectNumberingDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<PECC2ObjectNumbering> GetIQueryable()
        {
            return this.EDMsDataContext.PECC2ObjectNumbering;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PECC2ObjectNumbering> GetAll()
        {
            return this.EDMsDataContext.PECC2ObjectNumbering.ToList();
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
        public PECC2ObjectNumbering GetById(int id)
        {
            return this.EDMsDataContext.PECC2ObjectNumbering.FirstOrDefault(ob => ob.ID == id);
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
        public List<PECC2ObjectNumbering> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.PECC2ObjectNumbering.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(PECC2ObjectNumbering ob)
        {
            try
            {
                this.EDMsDataContext.AddToPECC2ObjectNumbering(ob);
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
        public bool Update(PECC2ObjectNumbering src)
        {
            try
            {
                PECC2ObjectNumbering des = (from rs in this.EDMsDataContext.PECC2ObjectNumbering
                                where rs.ID == src.ID
                                select rs).First();

                des.DocumentTypeId = src.DocumentTypeId;
                des.DocumentTypeName = src.DocumentTypeName;
                des.SegmentLenght = src.SegmentLenght;
                des.FullFormat = src.FullFormat;
                des.FullFormatName = src.FullFormatName;
                des.RealFormat = src.RealFormat;
                des.RealFormatName = src.RealFormatName;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;
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
        public bool Delete(PECC2ObjectNumbering src)
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
