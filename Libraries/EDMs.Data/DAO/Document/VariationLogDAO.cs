
namespace EDMs.Data.DAO.Document
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    public  class VariationLogDAO: BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VariationLogDAO"/> class.
        /// </summary>
        public VariationLogDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<VariationLog> GetIQueryable()
        {
            return this.EDMsDataContext.VariationLogs;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<VariationLog> GetAll()
        {
            return this.EDMsDataContext.VariationLogs.ToList();
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
        public VariationLog GetById(Guid id)
        {
            return this.EDMsDataContext.VariationLogs.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(VariationLog ob)
        {
            try
            {
                this.EDMsDataContext.AddToVariationLogs(ob);
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
        public bool Update(VariationLog src)
        {
            try
            {
                VariationLog des = (from rs in this.EDMsDataContext.VariationLogs
                              where rs.ID == src.ID
                              select rs).First();

                des.Title = src.Title;
                des.InstructionProposal = src.InstructionProposal;
                des.Order = src.Order;
                des.System = src.System;
                des.ContractRequirement = src.ContractRequirement;
                des.Description = src.Description;
                des.CostImpact = src.CostImpact;
                des.scheduleImpact = src.scheduleImpact;
                des.IssuedDate = src.IssuedDate;
                des.OtherAttachment = src.OtherAttachment;
                des.Remark = src.Remark;
                des.ProjectId = src.ProjectId;
                des.ProjectCode = src.ProjectCode;
                des.IsDelete = src.IsDelete;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
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
        public bool Delete(VariationLog src)
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
