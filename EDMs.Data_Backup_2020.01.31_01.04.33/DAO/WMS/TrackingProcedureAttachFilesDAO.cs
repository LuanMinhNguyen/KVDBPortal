﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TrackingProcedureAttachFileDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.WMS
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class TrackingProcedureAttachFileDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingProcedureAttachFileDAO"/> class.
        /// </summary>
        public TrackingProcedureAttachFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<TrackingProcedureAttachFile> GetIQueryable()
        {
            return this.EDMsDataContext.TrackingProcedureAttachFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<TrackingProcedureAttachFile> GetAll()
        {
            return this.EDMsDataContext.TrackingProcedureAttachFiles.ToList();
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
        public TrackingProcedureAttachFile GetById(Guid id)
        {
            return this.EDMsDataContext.TrackingProcedureAttachFiles.FirstOrDefault(ob => ob.ID == id);
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
        public List<TrackingProcedureAttachFile> GetSpecific(Guid tranId)
        {
            return this.EDMsDataContext.TrackingProcedureAttachFiles.Where(t => t.ID == tranId).ToList();
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
        public Guid? Insert(TrackingProcedureAttachFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToTrackingProcedureAttachFiles(ob);
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
        public bool Update(TrackingProcedureAttachFile src)
        {
            try
            {
                TrackingProcedureAttachFile des = (from rs in this.EDMsDataContext.TrackingProcedureAttachFiles
                                where rs.ID == src.ID
                                select rs).First();

                des.Filename = src.Filename;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
                des.IsDefault = src.IsDefault;
                des.Description = src.Description;
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
        public bool Delete(TrackingProcedureAttachFile src)
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