using System.Collections.Generic;
using System.Linq;
using EAM.Data.Entities;

namespace EAM.Data.DAO.Security
{
    public class AA_RolesDAO : BaseDAO
    {
        public AA_RolesDAO() : base() { }

        #region GET (Basic)
        public IQueryable<AA_Roles> GetIQueryable()
        {
            return this.EDMsDataContext.AA_Roles;
        }
        
        public List<AA_Roles> GetAll()
        {
            return this.EDMsDataContext.AA_Roles.OrderBy(t => t.Name).ToList();
        }

        public List<AA_Roles> GetAllSpecial()
        {
            return this.EDMsDataContext.AA_Roles.Where(t => t.IsAdmin == false).OrderBy(t => t.Name).ToList();
        }

        public AA_Roles GetByID(int ID)
        {
            return this.EDMsDataContext.AA_Roles.FirstOrDefault(ob => ob.Id == ID);
        }
       
        #endregion

        #region Get (Advances)

        #endregion

        #region Insert, Update, Delete
        public bool Insert(AA_Roles ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_Roles(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(AA_Roles src)
        {
            try
            {
                var des = (from rs in this.EDMsDataContext.AA_Roles
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

        public bool Delete(AA_Roles ob)
        {
            try
            {
                AA_Roles _ob = this.GetByID(ob.Id);
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
                AA_Roles _ob = this.GetByID(ID);
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
