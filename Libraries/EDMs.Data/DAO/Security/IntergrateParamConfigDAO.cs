// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IntergrateParamConfigDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.Security
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class IntergrateParamConfigDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntergrateParamConfigDAO"/> class.
        /// </summary>
        public IntergrateParamConfigDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<IntergrateParamConfig> GetIQueryable()
        {
            return this.EDMsDataContext.IntergrateParamConfigs;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<IntergrateParamConfig> GetAll()
        {
            return this.EDMsDataContext.IntergrateParamConfigs.OrderByDescending(t => t.ID).ToList();
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
        public IntergrateParamConfig GetById(int id)
        {
            return this.EDMsDataContext.IntergrateParamConfigs.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        #endregion

        #region Insert, Update, Delete

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
        public bool Update(IntergrateParamConfig src)
        {
            try
            {
                IntergrateParamConfig des = (from rs in this.EDMsDataContext.IntergrateParamConfigs
                                where rs.ID == src.ID
                                select rs).First();

                des.Amos_Dns = src.Amos_Dns;
                des.Amos_DataFile = src.Amos_DataFile;
                des.Amos_Pwd = src.Amos_Pwd;
                des.Amos_Uid = src.Amos_Uid;

                des.Sync_EmailSendExport = src.Sync_EmailSendExport;
                des.Sync_ExportFolder = src.Sync_ExportFolder;
                des.Sync_ImportFolder = src.Sync_ImportFolder;
                des.Sync_CurrentLocationId = src.Sync_CurrentLocationId;
                des.Sync_DefaultEmail = src.Sync_DefaultEmail;
                des.Sync_EmailName = src.Sync_EmailName;
                des.Sync_EmailPwd = src.Sync_EmailPwd;
                des.Sync_MailServer = src.Sync_MailServer;
                des.Sync_Port = src.Sync_Port;
                des.Sync_EnableSsl = src.Sync_EnableSsl;
                des.Sync_UseDefaultCredentials = src.Sync_UseDefaultCredentials;
                des.Sync_ImportConnStr = src.Sync_ImportConnStr;

                des.Sync_PopPort = src.Sync_PopPort;
                des.Sync_PopServer = src.Sync_PopServer;
                des.IsAutoGetSend = src.IsAutoGetSend;
                des.IsEnableSendEmailNotification = src.IsEnableSendEmailNotification;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        
        #endregion
    }
}
