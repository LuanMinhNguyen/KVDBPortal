namespace EDMs.Data.DAO.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class RoleDAO : BaseDAO
    {
        public RoleDAO() : base() { }

        #region GET (Basic)
        public IQueryable<Role> GetIQueryable()
        {
            return this.EDMsDataContext.Roles;
        }
        
        public List<Role> GetAll()
        {
            return this.EDMsDataContext.Roles.OrderBy(t => t.Name).ToList();
        }

        public List<Role> GetAllSpecial()
        {
            return this.EDMsDataContext.Roles.Where(t => t.IsAdmin == false).OrderBy(t => t.Name).ToList();
        }

        public Role GetByID(int ID)
        {
            return this.EDMsDataContext.Roles.FirstOrDefault(ob => ob.Id == ID);
        }
       
        #endregion

        #region Get (Advances)

        #endregion

        #region Insert, Update, Delete
        public bool Insert(Role ob)
        {
            try
            {
                this.EDMsDataContext.AddToRoles(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Role src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.Roles
                       where rs.Id == src.Id
                       select rs).First();

                des.Name = src.Name;
                des.Description = src.Description;
                des.IsAdmin = src.IsAdmin;
                des.IsUpdate = src.IsUpdate;
                des.TypeId = src.TypeId;
                des.TypeName = src.TypeName;
                des.Color = src.Color;
                des.IsLimitedView = src.IsLimitedView;
                des.ContractorId = src.ContractorId;
                des.ContractorName = src.ContractorName;
                des.IsAllowDelete = src.IsAllowDelete;
                des.IsInternal = src.IsInternal;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(Role ob)
        {
            try
            {
                Role _ob = this.GetByID(ob.Id);
                if (_ob != null)
                {
                    this.EDMsDataContext.DeleteObject(_ob);
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
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                Role _ob = this.GetByID(ID);
                if (_ob != null)
                {
                    this.EDMsDataContext.DeleteObject(_ob);
                    this.EDMsDataContext.SaveChanges();
                    return true;
                }
                else
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
