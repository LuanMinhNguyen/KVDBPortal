// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScopeProjectDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Scope
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class ScopeProjectDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScopeProjectDAO"/> class.
        /// </summary>
        public ScopeProjectDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ScopeProject> GetIQueryable()
        {
            return this.EDMsDataContext.ScopeProjects;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ScopeProject> GetAll()
        {
            return this.EDMsDataContext.ScopeProjects.OrderByDescending(t => t.ID).ToList();
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
        public ScopeProject GetById(int id)
        {
            return this.EDMsDataContext.ScopeProjects.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(ScopeProject ob)
        {
            try
            {
                this.EDMsDataContext.AddToScopeProjects(ob);
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
        public bool Update(ScopeProject src)
        {
            try
            {
                ScopeProject des = (from rs in this.EDMsDataContext.ScopeProjects
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.StartDate = src.StartDate;
                des.EndDate = src.EndDate;
                des.FrequencyForProgressChart = src.FrequencyForProgressChart;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;
                des.IsAutoCalculate = src.IsAutoCalculate;
                des.DCId = src.DCId;
                des.DCName = src.DCName;
                des.DocsFolderPath = src.DocsFolderPath;
                des.TransFolderPath = src.TransFolderPath;
                des.ReviewDuration = src.ReviewDuration;
                des.DCId = src.DCId;
                des.DCName = src.DCName;
                des.IsAutoDistribute = src.IsAutoDistribute;
                des.IsAutoSendNotification = src.IsAutoSendNotification;

                des.EmailAddress = src.EmailAddress;
                des.EmailName = src.EmailName;
                des.EmailPwd = src.EmailPwd;
                des.MailServer = src.MailServer;
                des.Port = src.Port;
                des.UseDefaultCredentials = src.UseDefaultCredentials;
                des.EnableSsl = src.EnableSsl;

                des.Code = src.Code;
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
        public bool Delete(ScopeProject src)
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
