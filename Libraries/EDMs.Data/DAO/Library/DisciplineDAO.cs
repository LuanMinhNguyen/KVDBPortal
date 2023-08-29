// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisciplineDAO.cs" company="">
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
    public class DisciplineDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DisciplineDAO"/> class.
        /// </summary>
        public DisciplineDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Discipline> GetIQueryable()
        {
            return this.EDMsDataContext.Disciplines;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Discipline> GetAll()
        {
            return this.EDMsDataContext.Disciplines.OrderByDescending(t => t.ID).ToList();
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
        public List<Discipline> GetAllByCategory(List<int> categoryIds)
        {
            return this.EDMsDataContext.Disciplines.ToArray().Where(t => t.CategoryId == null || t.CategoryId == 0 || categoryIds.Contains(t.CategoryId.GetValueOrDefault())).OrderBy(t => t.Name).ToList();
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
        public List<Discipline> GetAllByCategory(int categoryId)
        {
            return this.EDMsDataContext.Disciplines.Where(t => t.CategoryId == null || t.CategoryId == 0 || t.CategoryId == categoryId).OrderBy(t => t.ID).ToList();
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
        public Discipline GetById(int id)
        {
            return this.EDMsDataContext.Disciplines.FirstOrDefault(ob => ob.ID == id);
        }
        public Discipline GetByCode(string code)
        {
            return this.EDMsDataContext.Disciplines.FirstOrDefault(ob => ob.Code == code);
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
        public int? Insert(Discipline ob)
        {
            try
            {
                this.EDMsDataContext.AddToDisciplines(ob);
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
        public bool Update(Discipline src)
        {
            try
            {
                Discipline des = (from rs in this.EDMsDataContext.Disciplines
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.Weight = src.Weight;
                des.Complete = src.Complete;
                des.IsAutoCalculate = src.IsAutoCalculate;
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
        public bool Delete(Discipline src)
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
        /// The <see cref="Discipline"/>.
        /// </returns>
        public Discipline GetByName(string name, int projectId)
        {
            return this.EDMsDataContext.Disciplines.FirstOrDefault(ob => ob.Name.Trim() == name.Trim() && ob.ProjectId == projectId);
        }
    }
}
