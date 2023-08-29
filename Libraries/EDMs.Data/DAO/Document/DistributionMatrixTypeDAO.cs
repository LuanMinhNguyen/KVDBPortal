
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistributionMatrixTypeDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The category dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;

namespace EDMs.Data.DAO.Document
{
    /// <summary>
    /// The category dao.
    /// </summary>
    public class DistributionMatrixTypeDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionMatrixTypeDAO"/> class.
        /// </summary>
        public DistributionMatrixTypeDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DistributionMatrixType> GetIQueryable()
        {
            return this.EDMsDataContext.DistributionMatrixTypes;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DistributionMatrixType> GetAll()
        {
            return this.EDMsDataContext.DistributionMatrixTypes.ToList();
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
        public DistributionMatrixType GetById(int id)
        {
            return this.EDMsDataContext.DistributionMatrixTypes.FirstOrDefault(ob => ob.ID == id);
        }
        public DistributionMatrixType GetByName(string _Name)
        {
            return this.EDMsDataContext.DistributionMatrixTypes.FirstOrDefault(ob => ob.Name == _Name);
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
        public int? Insert(DistributionMatrixType ob)
        {
            try
            {
                this.EDMsDataContext.AddToDistributionMatrixTypes(ob);
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
        public bool Update(DistributionMatrixType src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.DistributionMatrixTypes
                           where rs.ID == src.ID
                           select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;
                des.Active = src.Active;
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
        public bool Delete(DistributionMatrixType src)
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
