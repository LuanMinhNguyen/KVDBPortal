// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DistributionMatrixDAO.cs" company="">
//   
// </copyright>
// <summary>
//   The DistributionMatrix dao.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    /// <summary>
    /// The DistributionMatrix dao.
    /// </summary>
    public class DistributionMatrixDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistributionMatrixDAO"/> class.
        /// </summary>
        public DistributionMatrixDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DistributionMatrix> GetAll()
        {
            return this.EDMsDataContext.DistributionMatrices.ToList();
        }
        public List<DistributionMatrix> GetAllList( List<int> listId)
        {
            return this.EDMsDataContext.DistributionMatrices.ToArray().Where(t=> listId.Contains(t.ID)).ToList();
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
        public DistributionMatrix GetById(int id)
        {
            return this.EDMsDataContext.DistributionMatrices.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(DistributionMatrix ob)
        {
            try
            {
                this.EDMsDataContext.AddToDistributionMatrices(ob);
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
        public bool Update(DistributionMatrix src)
        {
            try
            {
                DistributionMatrix des = (from rs in this.EDMsDataContext.DistributionMatrices
                                where rs.ID == src.ID
                                select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.ProjectName = src.ProjectName;
                des.ProjectId = src.ProjectId;
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
        public bool Delete(DistributionMatrix src)
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
