// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ToDoListDAO.cs" company="">
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
    public class ToDoListDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ToDoListDAO"/> class.
        /// </summary>
        public ToDoListDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ToDoList> GetIQueryable()
        {
            return this.EDMsDataContext.ToDoLists;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ToDoList> GetAll()
        {
            return this.EDMsDataContext.ToDoLists.ToList();
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
        public ToDoList GetById(int id)
        {
            return this.EDMsDataContext.ToDoLists.FirstOrDefault(ob => ob.ID == id);
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
        public List<ToDoList> GetSpecific(int tranId)
        {
            return this.EDMsDataContext.ToDoLists.Where(t => t.ID == tranId).ToList();
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
        public int? Insert(ToDoList ob)
        {
            try
            {
                this.EDMsDataContext.AddToToDoLists(ob);
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
        public bool Update(ToDoList src)
        {
            try
            {
                ToDoList des = (from rs in this.EDMsDataContext.ToDoLists
                                where rs.ID == src.ID
                                select rs).First();

                des.DocId = src.DocId;
                des.DocDisciplineId = src.DocDisciplineId;
                des.DocNumber = src.DocNumber;
                des.DocReceivedDate = des.DocReceivedDate;
                des.DocReceivedTransNo = des.DocReceivedTransNo;
                des.DocRevName = des.DocRevName;
                des.DocTitle = des.DocTitle;
                des.UserId = des.UserId;
                des.UserName = des.UserName;
                des.ActionName = des.ActionName;
                des.DeadlineDate = des.DeadlineDate;
                des.IsComplete = des.IsComplete;
                des.TaskTypeId = src.TaskTypeId;
                des.TaksTypeName = src.TaksTypeName;
                des.DocProjectId = src.DocProjectId;
                des.DocDisciplineName = src.DocDisciplineName;
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
        public bool Delete(ToDoList src)
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
