// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusDAO.cs" company="">
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
    public class StatusDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatusDAO"/> class.
        /// </summary>
        public StatusDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Status> GetIQueryable()
        {
            return this.EDMsDataContext.Status;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Status> GetAll()
        {
            return this.EDMsDataContext.Status.OrderByDescending(t => t.ID).ToList();
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
        public List<Status> GetAllByCategory(List<int> categoryIds)
        {
            return this.EDMsDataContext.Status.ToArray().Where(t => t.CategoryId == null || t.CategoryId == 0 || categoryIds.Contains(t.CategoryId.GetValueOrDefault())).ToList();
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
        public List<Status> GetAllByCategory(int categoryId)
        {
            return this.EDMsDataContext.Status.Where(t => t.CategoryId == null || t.CategoryId == 0 || t.CategoryId == categoryId).OrderBy(t => t.ID).ToList();
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
        public Status GetById(int id)
        {
            return this.EDMsDataContext.Status.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(Status ob)
        {
            try
            {
                this.EDMsDataContext.AddToStatus(ob);
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
        public bool Update(Status src)
        {
            try
            {
                Status des = (from rs in this.EDMsDataContext.Status
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.CategoryId = src.CategoryId;

                des.ProjectID = src.ProjectID;
                des.ProjectName = src.ProjectName;
                des.PercentCompleteDefault = src.PercentCompleteDefault;
                des.FinalCodeID = src.FinalCodeID;
                des.FinalCodeName = src.FinalCodeName;
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
        public bool Delete(Status src)
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

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="Status"/>.
        /// </returns>
        public Status GetByName(string name)
        {
            return this.EDMsDataContext.Status.FirstOrDefault(ob => ob.Name == name);
        }
    }
}
