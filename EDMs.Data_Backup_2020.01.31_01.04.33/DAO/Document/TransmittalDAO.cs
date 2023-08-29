// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TransmittalDAO.cs" company="">
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
    public class TransmittalDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TransmittalDAO"/> class.
        /// </summary>
        public TransmittalDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<Transmittal> GetIQueryable()
        {
            return this.EDMsDataContext.Transmittals;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<Transmittal> GetAll()
        {
            return this.EDMsDataContext.Transmittals.ToList();
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
        public List<Transmittal> GetAllByOwner(int userId)
        {
            return this.EDMsDataContext.Transmittals.Where(t => t.CreatedBy == userId || t.FromId == userId || t.ToId == userId).ToList();
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
        public Transmittal GetById(int id)
        {
            return this.EDMsDataContext.Transmittals.FirstOrDefault(ob => ob.ID == id);
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
        public List<Transmittal> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.Transmittals.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(Transmittal ob)
        {
            try
            {
                this.EDMsDataContext.AddToTransmittals(ob);
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
        public bool Update(Transmittal src)
        {
            try
            {
                Transmittal des = (from rs in this.EDMsDataContext.Transmittals
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.TransmittalNumber = src.TransmittalNumber;
                des.ProjectName = src.ProjectName;
                des.ContractName = src.ContractName;
                des.ToList = src.ToList;
                des.CCList = src.CCList;
                des.FromList = src.FromList;
                des.ToId = src.ToId;
                des.FromId = src.FromId;
                des.ReasonForIssue = src.ReasonForIssue;
                des.ReceivedDate = src.ReceivedDate;
                des.AttentionList = src.AttentionList;
                des.IssuseDate = src.IssuseDate;
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
        public bool Delete(Transmittal src)
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
