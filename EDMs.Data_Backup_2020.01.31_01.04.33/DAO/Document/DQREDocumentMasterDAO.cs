
namespace EDMs.Data.DAO.Document
{
    using System.Collections.Generic;
    using System.Linq;
    using System;

    using EDMs.Data.Entities;
    public class DQREDocumentMasterDAO : BaseDAO
    {/// <summary>
     /// Initializes a new instance of the <see cref="DQREDocumentMasterDAO"/> class.
     /// </summary>
        public DQREDocumentMasterDAO() : base() { }

        #region GET (Basic)

        /// <summary>
        /// The get i queryable.
        /// </summary>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        public IQueryable<DQREDocumentMaster> GetIQueryable()
        {
            return this.EDMsDataContext.DQREDocumentMasters;
        }

        /// <summary>
        /// The get all.
        /// </summary>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<DQREDocumentMaster> GetAll()
        {
            return this.EDMsDataContext.DQREDocumentMasters.OrderByDescending(t => t.ID).ToList();
        }

        public List<DQREDocumentMaster> GetByDiscipline(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.DisciplineId == codeID)
                    .ToList();
        }

        public List<DQREDocumentMaster> GetByPlant(int PlantID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.PlantId == PlantID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByArea(int AreaID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.AreaId == AreaID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByUnit(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.UnitId == codeID)
                    .ToList();
        }

        public List<DQREDocumentMaster> GetByDocumentType(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.DocumentTypeId == codeID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByMaterial(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.MaterialCodeId == codeID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByWork(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.WorkCodeId == codeID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByDrawing(int codeID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.DrawingCodeId == codeID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByOriginator(int OriginatorID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.OriginatorId == OriginatorID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByOriginatingOrganization(int OriginatorID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.OriginatingOrganizationId == OriginatorID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByReceivingOrganization(int OriginatorID)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.ReceivingOrganizationId == OriginatorID)
                    .ToList();
        }
        public List<DQREDocumentMaster> GetByDepartment(string DepartmentCode)
        {
            return
                this.EDMsDataContext.DQREDocumentMasters.Where(t => t.DepartmentCode == DepartmentCode)
                    .ToList();
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
        public DQREDocumentMaster GetById(Guid id)
        {
            return this.EDMsDataContext.DQREDocumentMasters.FirstOrDefault(ob => ob.ID == id);
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
        public Guid? Insert(DQREDocumentMaster ob)
        {
            try
            {
                this.EDMsDataContext.AddToDQREDocumentMasters(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch( Exception ex)
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
        public bool Update(DQREDocumentMaster src)
        {
            try
            {
                DQREDocumentMaster des = (from rs in this.EDMsDataContext.DQREDocumentMasters
                                          where rs.ID == src.ID
                                          select rs).First();

                des.SystemDocumentNo = src.SystemDocumentNo;
                des.Title = src.Title;
                des.OriginatorId = src.OriginatorId;
                des.OriginatorName = src.OriginatorName;
                des.OriginatingOrganizationId = src.OriginatingOrganizationId;
                des.OriginatingOrganizationName = src.OriginatingOrganizationName;
                des.ReceivingOrganizationId = src.ReceivingOrganizationId;
                des.ReceivingOrganizationName = src.ReceivingOrganizationName;
                des.DocumentTypeId = src.DocumentTypeId;
                des.DocumentTypeName = src.DocumentTypeName;
                des.DisciplineId = src.DisciplineId;
                des.DisciplineName = src.DisciplineName;
                des.MaterialCodeId = src.MaterialCodeId;
                des.MaterialCodeName = src.MaterialCodeName;
                des.WorkCodeId = src.WorkCodeId;
                des.WorkCodeName = src.WorkCodeName;
                des.DrawingCodeId = src.DrawingCodeId;
                des.DrawingCodeName = src.DrawingCodeName;
                des.EquipmentTagName = src.EquipmentTagName;
                des.DepartmentCode = src.DepartmentCode;
                des.MRSequenceNo = src.MRSequenceNo;
                des.DocumentSequenceNo = src.DocumentSequenceNo;
                des.SheetNo = src.SheetNo;
                des.PlantId = src.PlantId;
                des.PlantName = src.PlantName;
                des.AreaId = src.AreaId;
                des.AreaName = src.AreaName;
                des.UnitId = src.UnitId;
                des.UnitName = src.UnitName;

                des.IsDelete = src.IsDelete;

                des.LastUpdatedBy = src.LastUpdatedBy;
                des.LastUpdatedDate = src.LastUpdatedDate;

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
        public bool Delete(DQREDocumentMaster src)
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