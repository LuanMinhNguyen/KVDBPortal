// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommentResponseDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The CommentResponse dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The CommentResponse dao.
    /// </summary>
    public class CommentResponseDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentResponseDAO"/> class.
        /// </summary>
        public CommentResponseDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<CommentResponse> GetIQueryable()
        {
            return this.EDMsDataContext.CommentResponses;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<CommentResponse> GetAll()
        {
            return this.EDMsDataContext.CommentResponses.ToList();
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
        public CommentResponse GetById(int id)
        {
            return this.EDMsDataContext.CommentResponses.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(CommentResponse ob)
        {
            try
            {
                this.EDMsDataContext.AddToCommentResponses(ob);
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
        public bool Update(CommentResponse src)
        {
            try
            {
                CommentResponse des = (from rs in this.EDMsDataContext.CommentResponses
                                where rs.ID == src.ID
                                select rs).First();

                des.DocumentID = src.DocumentID;
                des.ManageSendDate = src.ManageSendDate;
                des.ManageSendTransID = src.ManageSendTransID;
                des.ManageSendTransNumber = src.ManageSendTransNumber;

                des.PlanReceiveDate = src.PlanReceiveDate;
                des.ActualReceiveDate = src.ActualReceiveDate;

                des.FromContractorID = src.FromContractorID;
                des.FromContractorName = src.FromContractorName;

                des.ToContractorID = src.ToContractorID;
                des.ToContractorName = src.ToContractorName;

                des.ReceiveCodeID = src.ReceiveCodeID;
                des.ReceiveCodeName = src.ReceiveCodeName;

                des.ReceiveTransID = src.ReceiveTransID;
                des.ReceiveTransNumber = src.ReceiveTransNumber;

                des.CommentSheetID = src.CommentSheetID;
                des.CommentSheetNumber = src.CommentSheetNumber;

                des.FromContractorTypeID = src.FromContractorTypeID;

                des.PlanSendCMSToDesign = src.PlanSendCMSToDesign;
                des.ActualSendCMSToDesign = src.ActualSendCMSToDesign;
                des.SendCMSToDesignTransID = src.SendCMSToDesignTransID;
                des.SendCMSToDesignTransName = src.SendCMSToDesignTransName;

                des.ResActualDesignToManage = src.ResActualDesignToManage;
                des.ResActualManageToDesign = src.ResActualManageToDesign;
                des.ResActualManageToReview = src.ResActualManageToReview;
                des.ResActualReviewToManage = src.ResActualReviewToManage;
                
                des.ResDesignToManageTransID = src.ResDesignToManageTransID;
                des.ResDesignToManageTransName = src.ResDesignToManageTransName;
                
                des.ResManageToDesignTransID = src.ResManageToDesignTransID;
                des.ResManageToDesignTransName = src.ResManageToDesignTransName;

                des.ResManageToReviewTransID = src.ResManageToReviewTransID;
                des.ResManageToReviewTransName = src.ResManageToReviewTransName;

                des.ResReviewToManageTransID = src.ResReviewToManageTransID;
                des.ResReviewToManageTransName = src.ResReviewToManageTransName;

                des.ResPlanDesignToManage = src.ResPlanDesignToManage;
                des.ResPlanManageToDesign = src.ResPlanManageToDesign;
                des.ResPlanManageToReview = src.ResPlanManageToReview;
                des.ResPlanReviewToManage = src.ResPlanReviewToManage;
                
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;

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
        public bool Delete(CommentResponse src)
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
