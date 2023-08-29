// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EAMWorkRequestDAO.cs" company="">
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
    public class EAMWorkRequestDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EAMWorkRequestDAO"/> class.
        /// </summary>
        public EAMWorkRequestDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<EAMWorkRequest> GetIQueryable()
        {
            return this.EDMsDataContext.EAMWorkRequests;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<EAMWorkRequest> GetAll()
        {
            return this.EDMsDataContext.EAMWorkRequests.ToList();
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
        public EAMWorkRequest GetById(int id)
        {
            return this.EDMsDataContext.EAMWorkRequests.FirstOrDefault(ob => ob.ID == id);
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
        public List<EAMWorkRequest> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.EAMWorkRequests.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(EAMWorkRequest ob)
        {
            try
            {
                this.EDMsDataContext.AddToEAMWorkRequests(ob);
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
        public bool Update(EAMWorkRequest src)
        {
            try
            {
                EAMWorkRequest des = (from rs in this.EDMsDataContext.EAMWorkRequests
                                where rs.ID == src.ID
                                select rs).First();

                des.OrganizationId = src.OrganizationId;
                des.OrganizationName = src.OrganizationName;
                des.RequestName = src.RequestName;
                des.EquipmentId = src.EquipmentId;
                des.EquipmentName = src.EquipmentName;
                des.LocationId = src.LocationId;
                des.LocationName = src.LocationName;
                des.DepartmentId = src.DepartmentId;
                des.DepartmentName = src.DepartmentName;
                des.CreatedDate = src.CreatedDate;
                des.ReportDate = src.ReportDate;
                des.CreatedById = src.CreatedById;
                des.CreatedByName = src.CreatedByName;
                des.RequestTypeId = src.RequestTypeId;
                des.RequestTypeName = src.RequestTypeName;
                des.CategoryId = src.CategoryId;
                des.CategoryName = src.CategoryName;
                des.StatusId = src.StatusId;
                des.StatusName = src.StatusName;
                des.PriorityId = src.PriorityId;
                des.PriorityName = src.PriorityName;
                des.StartDate = src.StartDate;
                des.ProblemCode = src.ProblemCode;
                des.AssignTo = src.AssignTo;
                des.PlanComplete = src.PlanComplete;
                des.Description = src.Description;
                des.Note = src.Note;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedName = src.UpdatedName;
                des.UpdatedDate = src.UpdatedDate;
                des.ManagerId = src.ManagerId;
                des.ManagerName = src.ManagerName;

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
        public bool Delete(EAMWorkRequest src)
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
