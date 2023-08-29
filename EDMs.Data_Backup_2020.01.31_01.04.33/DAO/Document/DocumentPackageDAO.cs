// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentPackageDAO.cs" company="">
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
    public class DocumentPackageDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentPackageDAO"/> class.
        /// </summary>
        public DocumentPackageDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DocumentPackage> GetIQueryable()
        {
            return this.EDMsDataContext.DocumentPackages;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DocumentPackage> GetAll()
        {
            return this.EDMsDataContext.DocumentPackages.OrderByDescending(t => t.ID).ToList();
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
        public DocumentPackage GetById(int id)
        {
            return this.EDMsDataContext.DocumentPackages.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(DocumentPackage ob)
        {
            try
            {
                this.EDMsDataContext.AddToDocumentPackages(ob);
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
        public bool Update(DocumentPackage src)
        {
            try
            {
                DocumentPackage des = (from rs in this.EDMsDataContext.DocumentPackages
                                where rs.ID == src.ID
                                select rs).First();

                des.DocNo = src.DocNo;
                des.DocTitle = src.DocTitle;
                des.WorkgroupId = src.WorkgroupId;
                des.WorkgroupName = src.WorkgroupName;
                des.DeparmentId = src.DeparmentId;
                des.DeparmentName = src.DeparmentName;
                des.StartDate = src.StartDate;
                des.PlanedDate = src.PlanedDate;
                des.RevisionId = src.RevisionId;
                des.RevisionName = src.RevisionName;
                des.RevisionActualDate = src.RevisionActualDate;
                des.RevisionPlanedDate = src.RevisionPlanedDate;
                des.RevisionCommentCode = src.RevisionCommentCode;
                des.RevisionReceiveTransNo = src.RevisionReceiveTransNo;
                des.Complete = src.Complete;
                des.Weight = src.Weight;
                des.OutgoingTransNo = src.OutgoingTransNo;
                des.OutgoingTransDate = src.OutgoingTransDate;
                des.IncomingTransDate = src.IncomingTransDate;
                des.IncomingTransNo = src.IncomingTransNo;
                des.ICAReviewCode = src.ICAReviewCode;
                des.ICAReviewOutTransNo = src.ICAReviewOutTransNo;
                des.ICAReviewReceivedDate = src.ICAReviewReceivedDate;
                des.Notes = src.Notes;
                des.DocumentTypeId = src.DocumentTypeId;
                des.DocumentTypeName = src.DocumentTypeName;
                des.DisciplineId = src.DisciplineId;
                des.DisciplineName = src.DisciplineName;
                des.PackageId = src.PackageId;
                des.PackageName = src.PackageName;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.IsLeaf = src.IsLeaf;
                des.IsDelete = src.IsDelete;
                des.IsEMDR = src.IsEMDR;
                des.IndexNumber = src.IndexNumber;

                des.IsCriticalDoc = src.IsCriticalDoc;
                des.IsPriorityDoc = src.IsPriorityDoc;
                des.IsVendorDoc = src.IsVendorDoc;

                des.ReferenceFromID = src.ReferenceFromID;
                des.ReferenceFromName = src.ReferenceFromName;
                des.StatusID = src.StatusID;
                des.StatusName = src.StatusName;
                des.FinalCodeID = src.FinalCodeID;
                des.FinalCodeName = src.FinalCodeName;
                des.CompleteForProject = src.CompleteForProject;

                des.ProjectFullName = src.ProjectFullName;
                des.DocTypeFullName = src.DocTypeFullName;
                des.DisciplineFullName = src.DisciplineFullName;
                des.OriginatorFullName = src.OriginatorFullName;
                des.OriginatorId = src.OriginatorId;
                des.OriginatorName = src.OriginatorName;
                des.SequencetialNumber = src.SequencetialNumber;
                des.DrawingSheetNumber = src.DrawingSheetNumber;

                des.FinalIssuePlanDate = src.FinalIssuePlanDate;
                des.FinalIssueActualDate = src.FinalIssueActualDate;
                des.FinalIssueTransNo = src.FinalIssueTransNo;

                des.FirstIssueActualDate = src.FirstIssueActualDate;
                des.FirstIssuePlanDate = src.FirstIssuePlanDate;
                des.FirstIssueTransNo = src.FirstIssueTransNo;

                des.RevisionActualReceiveCommentDate = src.RevisionActualReceiveCommentDate;
                des.RevisionPlanReceiveCommentDate = src.RevisionPlanReceiveCommentDate;
                des.IsInternalDocument = src.IsInternalDocument;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedDate = src.UpdatedDate;

                des.StatusFullName = src.StatusFullName;

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
        public bool Delete(DocumentPackage src)
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
