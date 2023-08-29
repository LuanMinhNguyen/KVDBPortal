﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NotificationRuleDAO.cs" company="">
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
    public class NotificationRuleDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationRuleDAO"/> class.
        /// </summary>
        public NotificationRuleDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<NotificationRule> GetIQueryable()
        {
            return this.EDMsDataContext.NotificationRules;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<NotificationRule> GetAll()
        {
            return this.EDMsDataContext.NotificationRules.ToList();
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
        public NotificationRule GetById(int id)
        {
            return this.EDMsDataContext.NotificationRules.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region GET ADVANCE

        /// <summary>
        /// The get all by discipline.
        /// </summary>
        /// <param name="disciplineId">
        /// The discipline id.
        /// </param>
        /// <returns>
        /// The <see cref="NotificationRule"/>.
        /// </returns>
        public NotificationRule GetAllByDiscipline(int disciplineId)
        {
            return this.EDMsDataContext.NotificationRules.FirstOrDefault(t => t.DisciplineID == disciplineId);
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
        /// The <see cref="int?"/>.
        /// </returns>
        public int? Insert(NotificationRule ob)
        {
            try
            {
                this.EDMsDataContext.AddToNotificationRules(ob);
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
        public bool Update(NotificationRule src)
        {
            try
            {
                NotificationRule des = (from rs in this.EDMsDataContext.NotificationRules
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.DisciplineID = src.DisciplineID;
                des.ReceiverList = src.ReceiverList;
                des.ReceiverListId = src.ReceiverListId;
                des.ReceiveGroup = src.ReceiveGroup;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.LastUpdatedBy = src.LastUpdatedBy;

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
        public bool Delete(NotificationRule src)
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
