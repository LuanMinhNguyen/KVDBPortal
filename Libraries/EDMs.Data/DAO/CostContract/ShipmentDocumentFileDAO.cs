using System.Collections.Generic;
using System.Linq;
using EDMs.Data.Entities;
using System;
namespace EDMs.Data.DAO.CostContract
{
   public class ShipmentDocumentFileDAO : BaseDAO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShipmentDocumentFileDAO"/> class.
        /// </summary>
        public ShipmentDocumentFileDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<ShipmentDocumentFile> GetIQueryable()
        {
            return this.EDMsDataContext.ShipmentDocumentFiles;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<ShipmentDocumentFile> GetAll()
        {
            return this.EDMsDataContext.ShipmentDocumentFiles.OrderByDescending(t => t.ID).ToList();
        }

        /// <summary>
        ///  get all document of shipment
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public List<ShipmentDocumentFile> GetAllShipmentId(Guid ID)
        {
            return this.EDMsDataContext.ShipmentDocumentFiles.Where(t=> t.ShipmentID==ID).OrderByDescending(t => t.ID).ToList();
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
        public ShipmentDocumentFile GetById(int id)
        {
            return this.EDMsDataContext.ShipmentDocumentFiles.FirstOrDefault(ob => ob.ID == id);
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
        public int? Insert(ShipmentDocumentFile ob)
        {
            try
            {
                this.EDMsDataContext.AddToShipmentDocumentFiles(ob);
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
        public bool Update(ShipmentDocumentFile src)
        {
            try
            {
                ShipmentDocumentFile des = (from rs in this.EDMsDataContext.ShipmentDocumentFiles
                                      where rs.ID == src.ID
                                      select rs).First();

                des.ShipmentID = src.ShipmentID;
                des.ShipmentNumber = src.ShipmentNumber;
                des.FileName = src.FileName;
                des.Description = src.Description;
                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;
                des.ExtensionIcon = src.ExtensionIcon;
                des.FileSize = src.FileSize;
                des.Number = src.Number;
                des.Extension = src.Extension;
                des.FilePath = src.FilePath;
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
        public bool Delete(ShipmentDocumentFile src)
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
