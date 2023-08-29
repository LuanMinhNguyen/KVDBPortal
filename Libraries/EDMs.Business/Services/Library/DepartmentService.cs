

namespace EDMs.Business.Services.Library
{

    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
    using EDMs.Data.DAO.Library;

    public class DepartmentService
    {
        
        /// <summary>
        /// the repo
        /// </summary>
        private readonly DepartmentDAO repo;

        /// <summary>
        ///  initializes a new instance of the <see cref="DepartmentService"/> class.
        /// </summary>
        public DepartmentService()
        {

            this.repo = new DepartmentDAO();
        }
        /// <summary>
        /// the get all department.
        /// </summary>
        /// <returns></returns>
        public List<Department> GetAll()
        {
            return this.repo.GetAll().ToList();

        }
        /// <summary>
        /// the get department by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Department GetById(int id)
        {
            return this.repo.GetById(id);

        }

        public List<Department> GetAllByProject(int projectId)
        {
            return this.repo.GetAllByProject(projectId);
        }


        public Department GetByName(string name, int projectid)
        {
            return this.repo.GetAll().FirstOrDefault(t => t.Name.Trim() == name.Trim() && t.ProjectId == projectid);
        }
        /// <summary>
        /// the insert resounce
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public int? Insert(Department ob)
        {
            return this.repo.Insert(ob);
        }

        /// <summary>
        /// updete resource
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public bool Update(Department ob)
        {
            try
            {
                return this.repo.Update(ob);
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// delete resource
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        public bool Delete(Department ob)
        {
            try
            {
                return this.repo.Delete(ob);
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// delete resource by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(int id)
        {
            try
            {
                return this.repo.Delete(id);
            }
            catch
            {
                return false;
            }
        }
    }
}
