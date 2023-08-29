using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.DAO.Document
{
 

    using EDMs.Data.Entities;
    public  class RFIDAO : BaseDAO
    {
        public RFIDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<RFI> GetIQueryable()
        {
            return this.EDMsDataContext.RFIs;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<RFI> GetAll()
        {
            return this.EDMsDataContext.RFIs.ToList();
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
        public RFI GetById(Guid id)
        {
            return this.EDMsDataContext.RFIs.FirstOrDefault(ob => ob.ID == id);
        }
        /// <summary>
        /// get by number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public RFI GetByNumber(string number)
        {
            return this.EDMsDataContext.RFIs.FirstOrDefault(ob => ob.Number.ToLower() == number.ToLower());
        }
        public List<RFI> GetByIssudDate(DateTime issuedate)
        {
            return this.EDMsDataContext.RFIs.Where(ob => ob.IssuedDate.GetValueOrDefault().Date== issuedate.Date).ToList();
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
        public Guid? Insert(RFI ob)
        {
            try
            {
                this.EDMsDataContext.AddToRFIs(ob);
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
        public bool Update(RFI src)
        {
            try
            {
                RFI des = (from rs in this.EDMsDataContext.RFIs
                              where rs.ID == src.ID
                              select rs).First();
                des.Number = src.Number;
                des.GroupId = src.GroupId;
                des.GroupName = src.GroupName;
                des.Year = src.Year;
                des.SequentialNumber = src.SequentialNumber;
                des.Sequence = src.Sequence;
                des.SiteManager = src.SiteManager;
                des.QAQCManager = src.QAQCManager;
                des.IsDelete = src.IsDelete;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.ProjectId = src.ProjectId;
                des.ProjectCode = src.ProjectCode;
                des.IssuedDate = src.IssuedDate;
                des.IsWFComplete = src.IsWFComplete;
                des.IsInWFProcess = src.IsInWFProcess;
                des.IsCompleteFinal = src.IsCompleteFinal;
                des.CurrentAssignUserName = src.CurrentAssignUserName;
                des.CurrentWorkflowName = src.CurrentWorkflowName;
                des.CurrentWorkflowStepName = src.CurrentWorkflowStepName;
                des.IsUseCustomWfFromTrans = src.IsUseCustomWfFromTrans;
                des.IsUseIsUseCustomWfFromObj = src.IsUseIsUseCustomWfFromObj;
                des.IsAttachWorkflow = src.IsAttachWorkflow;
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
        public bool Delete(RFI src)
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
