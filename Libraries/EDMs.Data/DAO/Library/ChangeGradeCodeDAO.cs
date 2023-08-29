// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ChangeGradeCodeDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Library
{
    using System.Collections.Generic;
    using System.Linq;

    using Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class ChangeGradeCodeDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChangeGradeCodeDAO"/> class.
        /// </summary>
        public ChangeGradeCodeDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ChangeGradeCode> GetIQueryable()
        {
            return this.EDMsDataContext.ChangeGradeCodes;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ChangeGradeCode> GetAll()
        {
            return this.EDMsDataContext.ChangeGradeCodes.OrderByDescending(t => t.ID).ToList();
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
        public ChangeGradeCode GetById(int id)
        {
            return this.EDMsDataContext.ChangeGradeCodes.FirstOrDefault(ob => ob.ID == id);
        }
        public ChangeGradeCode GetByCode(string code)
        {
            return this.EDMsDataContext.ChangeGradeCodes.FirstOrDefault(ob => ob.Code == code);
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
        public int? Insert(ChangeGradeCode ob)
        {
            try
            {
                this.EDMsDataContext.AddToChangeGradeCodes(ob);
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
        public bool Update(ChangeGradeCode src)
        {
            try
            {
                ChangeGradeCode des = (from rs in this.EDMsDataContext.ChangeGradeCodes
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Code = src.Code;
                des.Description = src.Description;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.Active = src.Active;
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
        public bool Delete(ChangeGradeCode src)
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
