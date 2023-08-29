// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExportDataChangeHistoryDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.Security
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class ExportDataChangeHistoryDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDataChangeHistoryDAO"/> class.
        /// </summary>
        public ExportDataChangeHistoryDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ExportDataChangeHistory> GetIQueryable()
        {
            return this.EDMsDataContext.ExportDataChangeHistories;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ExportDataChangeHistory> GetAll()
        {
            return this.EDMsDataContext.ExportDataChangeHistories.OrderByDescending(t => t.ID).ToList();
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
        public ExportDataChangeHistory GetById(int id)
        {
            return this.EDMsDataContext.ExportDataChangeHistories.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

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
        public int? Insert(ExportDataChangeHistory ob)
        {
            try
            {
                this.EDMsDataContext.AddToExportDataChangeHistories(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
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
        public bool Delete(ExportDataChangeHistory src)
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

        public bool Update(ExportDataChangeHistory src)
        {
            try
            {
                ExportDataChangeHistory des = (from rs in this.EDMsDataContext.ExportDataChangeHistories
                                               where rs.ID == src.ID
                                               select rs).First();

                des.IsComplete = src.IsComplete;
                des.ErrorMess = src.ErrorMess;
                des.SendMailComplete = src.SendMailComplete;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
