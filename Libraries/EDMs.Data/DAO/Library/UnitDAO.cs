
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitDAO.cs" company="">
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
    public class UnitDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnitDAO"/> class.
        /// </summary>
        public UnitDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Unit> GetIQueryable()
        {
            return this.EDMsDataContext.Units;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Unit> GetAll()
        {
            return this.EDMsDataContext.Units.ToList();
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
        public Unit GetById(int id)
        {
            return this.EDMsDataContext.Units.FirstOrDefault(ob => ob.ID == id);
        }
        public Unit GetByName(string _Name)
        {
            return this.EDMsDataContext.Units.FirstOrDefault(ob => ob.Name == _Name);
        }

        public Unit GetByCode(string _Code)
        {
            return this.EDMsDataContext.Units.FirstOrDefault(ob => ob.Code == _Code);
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
        public int? Insert(Unit ob)
        {
            try
            {
                this.EDMsDataContext.AddToUnits(ob);
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
        public bool Update(Unit src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.Units
                           where rs.ID == src.ID
                           select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.Code = src.Code;
                des.AreaId = src.AreaId;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.ProjectName = src.ProjectName;
                des.ProjectId = src.ProjectId;
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
        public bool Delete(Unit src)
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
