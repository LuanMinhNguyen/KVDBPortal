﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WellDAO.cs" company="">
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
    public class WellDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WellDAO"/> class.
        /// </summary>
        public WellDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Well> GetIQueryable()
        {
            return this.EDMsDataContext.Wells;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Well> GetAll()
        {
            return this.EDMsDataContext.Wells.ToList();
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
        public Well GetById(int id)
        {
            return this.EDMsDataContext.Wells.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(Well ob)
        {
            try
            {
                this.EDMsDataContext.AddToWells(ob);
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
        public bool Update(Well src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.Wells
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.PlatformId = src.PlatformId;
                des.StartDate = src.StartDate;
                des.EndDate = src.EndDate;
                des.StartDateTesting = src.StartDateTesting;
                des.EndDateTesting = src.EndDateTesting;
                des.Active = src.Active;

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
        public bool Delete(Well src)
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
