

namespace EDMs.Data.DAO.Library
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
 public   class CustomizeReceivedEmailDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomizeReceivedEmailDAO"/> class.
        /// </summary>
        public CustomizeReceivedEmailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<CustomizeReceivedEmail> GetIQueryable()
        {
            return this.EDMsDataContext.CustomizeReceivedEmails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<CustomizeReceivedEmail> GetAll()
        {
            return this.EDMsDataContext.CustomizeReceivedEmails.ToList();
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
        public CustomizeReceivedEmail GetById(int id)
        {
            return this.EDMsDataContext.CustomizeReceivedEmails.FirstOrDefault(ob => ob.ID == id);
        }
        public CustomizeReceivedEmail GetByName(string _Name)
        {
            return this.EDMsDataContext.CustomizeReceivedEmails.FirstOrDefault(ob => ob.Name == _Name);
        }

        public CustomizeReceivedEmail GetByType(int typeid, int from)
        {
            return this.EDMsDataContext.CustomizeReceivedEmails.FirstOrDefault(ob => ob.TypeID==typeid && from==ob.Pecc2SendReceived);
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
        public int? Insert(CustomizeReceivedEmail ob)
        {
            try
            {
                this.EDMsDataContext.AddToCustomizeReceivedEmails(ob);
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
        public bool Update(CustomizeReceivedEmail src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.CustomizeReceivedEmails
                           where rs.ID == src.ID
                           select rs).First();

                des.Name = src.Name;
                des.ToUserIDs = src.ToUserIDs;
                des.RecipientsTo = src.RecipientsTo;
                des.CCUserIDs = src.CCUserIDs;
                des.DistributionMatrixCCIDs = src.DistributionMatrixCCIDs;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.ProjectName = src.ProjectName;
                des.ProjectID = src.ProjectID;
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
        public bool Delete(CustomizeReceivedEmail src)
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