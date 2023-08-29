// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PECC2TransmittalAttachDocFilesDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class PECC2TransmittalAttachDocFileDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PECC2TransmittalAttachDocFileDAO"/> clas.
        /// </summary>
        public PECC2TransmittalAttachDocFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<PECC2TransmittalAttachDocFiles> GetIQueryable()
        {
            return this.EDMsDataContext.PECC2TransmittalAttachDocFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<PECC2TransmittalAttachDocFiles> GetAll()
        {
            return this.EDMsDataContext.PECC2TransmittalAttachDocFiles.ToList();
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
        public PECC2TransmittalAttachDocFiles GetById(Guid id)
        {
            return this.EDMsDataContext.PECC2TransmittalAttachDocFiles.FirstOrDefault(ob => ob.ID == id);
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
        public List<PECC2TransmittalAttachDocFiles> GetSpecific(Guid tranId)
        {
            return this.EDMsDataContext.PECC2TransmittalAttachDocFiles.Where(t => t.ID == tranId).ToList();
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
        public Guid? Insert(PECC2TransmittalAttachDocFiles ob)
        {
            try
            {
                this.EDMsDataContext.AddToPECC2TransmittalAttachDocFiles(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
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
        /// True if delete succes, false if not
        /// </returns>
        public bool Delete(PECC2TransmittalAttachDocFiles src)
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
