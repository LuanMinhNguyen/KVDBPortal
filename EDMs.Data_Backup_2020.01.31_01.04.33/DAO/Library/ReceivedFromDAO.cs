// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReceivedFromDAO.cs" company="">
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

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class ReceivedFromDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivedFromDAO"/> class.
        /// </summary>
        public ReceivedFromDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ReceivedFrom> GetIQueryable()
        {
            return this.EDMsDataContext.ReceivedFroms;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ReceivedFrom> GetAll()
        {
            return this.EDMsDataContext.ReceivedFroms.OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryIds">
        /// The category ids.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ReceivedFrom> GetAllByCategory(List<int> categoryIds)
        {
            return this.EDMsDataContext.ReceivedFroms.ToArray().Where(t => t.CategoryId == null || t.CategoryId == 0 || categoryIds.Contains(t.CategoryId.GetValueOrDefault())).ToList();
        }

        /// <summary>
        /// The get all by category.
        /// </summary>
        /// <param name="categoryId">
        /// The category id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ReceivedFrom> GetAllByCategory(int categoryId)
        {
            return this.EDMsDataContext.ReceivedFroms.Where(t => t.CategoryId == null || t.CategoryId == 0 || t.CategoryId == categoryId).OrderBy(t => t.ID).ToList();
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
        public ReceivedFrom GetById(int id)
        {
            return this.EDMsDataContext.ReceivedFroms.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(ReceivedFrom ob)
        {
            try
            {
                this.EDMsDataContext.AddToReceivedFroms(ob);
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
        public bool Update(ReceivedFrom src)
        {
            try
            {
                ReceivedFrom des = (from rs in this.EDMsDataContext.ReceivedFroms
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.CategoryId = src.CategoryId;
                
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

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
        public bool Delete(ReceivedFrom src)
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

        public ReceivedFrom GetByName(string name)
        {
            return this.EDMsDataContext.ReceivedFroms.FirstOrDefault(t => t.Name == name);
        }
    }
}
