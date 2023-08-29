

namespace EDMs.Business.Services.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Document;
    using EDMs.Data.Entities;

    public class RFIAttachFileService
    {
        private readonly RFIAttachFileDAO repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="RFIAttachFileService"/> class.
        /// </summary>
        public RFIAttachFileService()
    {
        this.repo = new RFIAttachFileDAO();
    }

    #region Get (Advances)

    /// <summary>
    /// The get specific.
    /// </summary>
    /// <param name="tranId">
    /// The tran id.
    /// </param>
    /// <returns>
    /// The <see cref="List"/>.
    /// </returns>
    public List<RFIAttachFile> GetByRFI(Guid RFIId)
    {
        return this.repo.GetAllRFI(RFIId).ToList();
    }
    #endregion

    #region GET (Basic)
    /// <summary>
    /// Get All Categories
    /// </summary>
    /// <returns>
    /// The category
    /// </returns>
    public List<RFIAttachFile> GetAll()
    {
        return this.repo.GetAll().ToList();
    }

    /// <summary>
    /// Get Resource By ID
    /// </summary>
    /// <param name="id">
    /// ID of category
    /// </param>
    /// <returns>
    /// The category</returns>
    public RFIAttachFile GetById(Guid id)
    {
        return this.repo.GetById(id);
    }

    /// <summary>
    /// The get by name.
    /// </summary>
    /// <param name="name">
    /// The name.
    /// </param>
    /// <returns>
    /// The <see cref="RFIAttachFile"/>.
    /// </returns>
    public RFIAttachFile GetByNameServer(string nameServer)
    {
        return this.repo.GetAll().FirstOrDefault(t => t.FilePath.ToLower().Contains(nameServer.ToLower()));
    }
    #endregion

    #region Insert, Update, Delete
    /// <summary>
    /// Insert Resource
    /// </summary>
    /// <param name="bo"></param>
    /// <returns></returns>
    public Guid? Insert(RFIAttachFile bo)
    {
        return this.repo.Insert(bo);
    }

    /// <summary>
    /// Update Resource
    /// </summary>
    /// <param name="bo"></param>
    /// <returns></returns>
    public bool Update(RFIAttachFile bo)
    {
        try
        {
            return this.repo.Update(bo);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Delete Resource
    /// </summary>
    /// <param name="bo"></param>
    /// <returns></returns>
    public bool Delete(RFIAttachFile bo)
    {
        try
        {
            return this.repo.Delete(bo);
        }
        catch (Exception)
        {
            return false;
        }
    }

    /// <summary>
    /// Delete Resource By ID
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool Delete(Guid id)
    {
        try
        {
            return this.repo.Delete(id);
        }
        catch (Exception)
        {
            return false;
        }
    }
    #endregion
}
}
