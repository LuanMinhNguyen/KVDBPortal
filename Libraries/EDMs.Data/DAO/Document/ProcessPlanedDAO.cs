﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcessPlanedDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The ProcessPlaned dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The ProcessPlaned dao.
    /// </summary>
    public class ProcessPlanedDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessPlanedDAO"/> class.
        /// </summary>
        public ProcessPlanedDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ProcessPlaned> GetIQueryable()
        {
            return this.EDMsDataContext.ProcessPlaneds;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ProcessPlaned> GetAll()
        {
            return this.EDMsDataContext.ProcessPlaneds.ToList();
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
        public ProcessPlaned GetById(int id)
        {
            return this.EDMsDataContext.ProcessPlaneds.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(ProcessPlaned ob)
        {
            try
            {
                this.EDMsDataContext.AddToProcessPlaneds(ob);
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
        public bool Update(ProcessPlaned src)
        {
            try
            {
                ProcessPlaned des = (from rs in this.EDMsDataContext.ProcessPlaneds
                                where rs.ID == src.ID
                                select rs).First();

                des.ProjectId = src.ProjectId;
                des.WorkgroupId = src.WorkgroupId;

                des.Planed = src.Planed;

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
        public bool Delete(ProcessPlaned src)
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
