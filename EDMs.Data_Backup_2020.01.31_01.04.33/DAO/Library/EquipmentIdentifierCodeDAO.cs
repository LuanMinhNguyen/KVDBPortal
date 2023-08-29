// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EquipmentIdentifierCodeDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Library
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The category dao.
    /// </summary>
    public class EquipmentIdentifierCodeDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EquipmentIdentifierCodeDAO"/> class.
        /// </summary>
        public EquipmentIdentifierCodeDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<EquipmentIdentifierCode> GetIQueryable()
        {
            return this.EDMsDataContext.EquipmentIdentifierCodes;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<EquipmentIdentifierCode> GetAll()
        {
            return this.EDMsDataContext.EquipmentIdentifierCodes.OrderByDescending(t => t.ID).ToList();
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
        public EquipmentIdentifierCode GetById(int id)
        {
            return this.EDMsDataContext.EquipmentIdentifierCodes.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(EquipmentIdentifierCode ob)
        {
            try
            {
                this.EDMsDataContext.AddToEquipmentIdentifierCodes(ob);
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
        public bool Update(EquipmentIdentifierCode src)
        {
            try
            {
                EquipmentIdentifierCode des = (from rs in this.EDMsDataContext.EquipmentIdentifierCodes
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.TypicalExample = src.TypicalExample;
                des.Classification = src.Classification;
                des.Active = src.Active;
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
        public bool Delete(EquipmentIdentifierCode src)
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

        /// <summary>
        /// The get by name.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <returns>
        /// The <see cref="EquipmentIdentifierCode"/>.
        /// </returns>
        public EquipmentIdentifierCode GetByName(string name, int projectId)
        {
            return this.EDMsDataContext.EquipmentIdentifierCodes.FirstOrDefault(ob => ob.Name.Trim() == name.Trim() && ob.ProjectId == projectId);
        }
    }
}
