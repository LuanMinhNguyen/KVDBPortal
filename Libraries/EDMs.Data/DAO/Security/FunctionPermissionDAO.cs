namespace EDMs.Data.DAO.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class FunctionPermissionDAO : BaseDAO
    {
        public FunctionPermissionDAO() : base() { }

        #region GET (Basic)
        public IQueryable<FunctionPermission> GetIQueryable()
        {
            return this.EDMsDataContext.FunctionPermissions;
        }

        public List<FunctionPermission> GetAll()
        {
            return this.EDMsDataContext.FunctionPermissions.ToList();
        }

        public FunctionPermission GetById(int id)
        {
            return this.EDMsDataContext.FunctionPermissions.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region Get (Advances)


        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the FunctionPermissions.
        /// </summary>
        /// <param name="FunctionPermissions">The FunctionPermissions.</param>
        /// <returns></returns>
        public bool DeleteFunctionPermissions(List<FunctionPermission> FunctionPermissions)
        {
            try
            {
                foreach (var item in FunctionPermissions)
                {
                    this.EDMsDataContext.DeleteObject(item);
                    this.EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Adds the FunctionPermissions.
        /// </summary>
        /// <param name="FunctionPermissions">The FunctionPermissions.</param>
        /// <returns></returns>
        public bool AddFunctionPermissions(List<FunctionPermission> FunctionPermissions)
        {
            try
            {
                foreach (var item in FunctionPermissions)
                {
                    this.EDMsDataContext.AddToFunctionPermissions(item);
                    this.EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(FunctionPermission src)
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
        /// <param name="id"></param>
        /// ID of entity
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                var des = this.GetById(id);
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


        public int? Insert(FunctionPermission ob)
        {
            try
            {
                this.EDMsDataContext.AddToFunctionPermissions(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(FunctionPermission src)
        {
            try
            {
                FunctionPermission des;

                des = (from rs in this.EDMsDataContext.FunctionPermissions
                       where rs.ID == src.ID
                       select rs).First();

                des.UserId = src.UserId;
                des.FullName = src.FullName;
                des.DeptId = src.DeptId;
                des.DeptName = src.DeptName;
                des.ObjectTypeId = src.ObjectTypeId;
                des.ObjectTypeName = src.ObjectTypeName;
                des.IsView = src.IsView;
                des.IsCreate = src.IsCreate;
                des.IsUpdate = src.IsUpdate;
                des.IsCancel = src.IsCancel;
                des.IsAttachWorkflow = src.IsAttachWorkflow;
                des.UpdatedDate = src.UpdatedDate;
                des.UpdatedBy = src.UpdatedBy;
                des.UpdatedByName = src.UpdatedByName;


                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

    }
}
