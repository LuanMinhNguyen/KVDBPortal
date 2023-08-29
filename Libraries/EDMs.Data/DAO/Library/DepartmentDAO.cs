

using Telerik.Web.UI;

namespace EDMs.Data.DAO.Library
{
    using EDMs.Data.Entities;

    using System.Collections.Generic;
    using System.Linq;

    public class DepartmentDAO : BaseDAO
    {
        public DepartmentDAO() : base() { }

        /// <summary>
        /// this get i queryable.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Department> GetIqueryAble()
        {
            return this.EDMsDataContext.Departments;
        }
        /// <summary>
        /// the get all.
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAll()
        {
            return this.EDMsDataContext.Departments.ToList();
        }
        /// <summary>
        /// the get by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Department GetById(int id)
        {
            return this.EDMsDataContext.Departments.FirstOrDefault(ob => ob.ID == id);

        }

        public List<Department> GetAllByProject(int projectId)
        {
            return this.EDMsDataContext.Departments.Where(t => t.ProjectId == null || t.ProjectId == 0 || t.ProjectId == projectId).OrderBy(t => t.ID).ToList();

        }
        /// <summary>
        /// the insert.
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public int? Insert(Department ob)
        {
            try
            {
                this.EDMsDataContext.AddToDepartments(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// the Update.
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public bool Update(Department ob)
        {
            try
            {

                var des = (from rs in this.EDMsDataContext.Departments where rs.ID == ob.ID select rs).First();
                des.Name = ob.Name;
                des.Description = ob.Description;
                des.ProjectId = ob.ProjectId;
                des.ProjectName = ob.ProjectName;
                des.LastUpdatedBy = ob.LastUpdatedBy;
                des.LastUpdatedDate = ob.LastUpdatedDate;
                des.TypeId = ob.TypeId;
                des.TypeName = ob.TypeName;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// the delete by departmentcode.
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>

        public bool Delete(Department ob)
        {
            try
            {

                var des = this.GetById(ob.ID);
                if (des != null)
                {
                    this.EDMsDataContext.DeleteObject(des);
                    this.EDMsDataContext.SaveChanges();

                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// the delete by id.
        /// </summary>
        /// <param name="id"></param>
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
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}