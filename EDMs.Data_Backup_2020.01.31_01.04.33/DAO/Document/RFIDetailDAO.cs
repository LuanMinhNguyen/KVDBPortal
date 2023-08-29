using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.DAO.Document
{
    using EDMs.Data.Entities;
    public class RFIDetailDAO : BaseDAO
    {
        public RFIDetailDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<RFIDetail> GetIQueryable()
        {
            return this.EDMsDataContext.RFIDetails;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<RFIDetail> GetAll()
        {
            return this.EDMsDataContext.RFIDetails.ToList();
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
        public RFIDetail GetById(Guid id)
        {
            return this.EDMsDataContext.RFIDetails.FirstOrDefault(ob => ob.ID == id);
        }

        public List<RFIDetail> GetAllRFIID(Guid rfiId)
        {
            return this.EDMsDataContext.RFIDetails.Where(t => t.RFIID == rfiId).ToList();
        }
        /// <summary>
        /// get by number
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public RFIDetail GetByNumber(int number)
        {
            return this.EDMsDataContext.RFIDetails.FirstOrDefault(ob => ob.Number == number);
        }
        public List<RFIDetail> GetByIssudDate(DateTime time)
        {
            return this.EDMsDataContext.RFIDetails.Where(ob => ob.Time.GetValueOrDefault() == time).ToList();
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
        public Guid? Insert(RFIDetail ob)
        {
            try
            {
                this.EDMsDataContext.AddToRFIDetails(ob);
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
        public bool Update(RFIDetail src)
        {
            try
            {
                RFIDetail des = (from rs in this.EDMsDataContext.RFIDetails
                           where rs.ID == src.ID
                           select rs).First();
                des.Number = src.Number;
                des.GroupId = src.GroupId;
                des.GroupName = src.GroupName;
                des.WorkTitle = src.WorkTitle;
                des.Description = src.Description;
                des.Location = src.Location;
                des.InspectionTypeId = src.InspectionTypeId;
                des.InspectionTypeName = src.InspectionTypeName;
                des.ContractorContact = src.ContractorContact;
                des.Remark = src.Remark;
                des.EngineeringActionID = src.EngineeringActionID;
                des.EngineeringActionName = src.EngineeringActionName;
                des.Time = src.Time;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedByName = src.LastUpdatedByName;
                des.RFIID = src.RFIID;
                des.RFINo = src.RFINo;
                des.CommentContent = src.CommentContent;
              
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
        public bool Delete(RFIDetail src)
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
