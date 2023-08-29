// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IncomingTransmittalDAO.cs" company="">
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
    public class IncomingTransmittalDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IncomingTransmittalDAO"/> class.
        /// </summary>
        public IncomingTransmittalDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<IncomingTransmittal> GetIQueryable()
        {
            return this.EDMsDataContext.IncomingTransmittals;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<IncomingTransmittal> GetAll()
        {
            return this.EDMsDataContext.IncomingTransmittals.ToList();
        }

        /// <summary>
        /// The get all by owner.
        /// </summary>
        /// <param name="createdBy">
        /// The created by.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<IncomingTransmittal> GetAllByOwner(int createdBy)
        {
            return this.EDMsDataContext.IncomingTransmittals.Where(t => t.CreatedBy == createdBy).ToList();
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
        public IncomingTransmittal GetById(int id)
        {
            return this.EDMsDataContext.IncomingTransmittals.FirstOrDefault(ob => ob.ID == id);
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
        public List<IncomingTransmittal> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.IncomingTransmittals.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(IncomingTransmittal ob)
        {
            try
            {
                this.EDMsDataContext.AddToIncomingTransmittals(ob);
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
        public bool Update(IncomingTransmittal src)
        {
            try
            {
                IncomingTransmittal des = (from rs in this.EDMsDataContext.IncomingTransmittals
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.TransNumber = src.TransNumber;
                des.ProjectName = src.ProjectName;
                des.ProjectId = src.ProjectId;
                des.FromId = src.FromId;
                des.FromName = src.FromName;
                des.ReceivedDate = src.ReceivedDate;
                des.ToId = src.ToId;
                des.ToName = src.ToName;
                des.AttentionId = src.AttentionId;
                des.AttentionName = src.AttentionName;
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
        public bool Delete(IncomingTransmittal src)
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
