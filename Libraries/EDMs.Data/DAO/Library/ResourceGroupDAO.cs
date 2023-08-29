namespace EDMs.Data.DAO.Library
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class ResourceGroupDAO : BaseDAO
    {
        public ResourceGroupDAO() : base() { }

        #region GET (Basic)
        public IQueryable<ResourceGroup> GetIQueryable()
        {
            return this.EDMsDataContext.ResourceGroups;
        }
        
        public List<ResourceGroup> GetAll()
        {
            return this.EDMsDataContext.ResourceGroups.ToList<ResourceGroup>();
        }

        public ResourceGroup GetByID(int ID)
        {
            return this.EDMsDataContext.ResourceGroups.Where(ob => ob.Id == ID).FirstOrDefault<ResourceGroup>();
        }
       
        #endregion

        #region GET ADVANCE
        
        #endregion

        #region Insert, Update, Delete
        public bool Insert(ResourceGroup ob)
        {
            try
            {
                this.EDMsDataContext.AddToResourceGroups(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(ResourceGroup ob)
        {
            try
            {
                //ResourceGroup _ob = GetByID(ob.Id);
                //if (_ob != null)
                //{
                //    EDMsDataContext.ApplyCurrentValues(_ob.EntityKey.EntitySetName, ob);
                //    EDMsDataContext.SaveChanges();
                //    return true;
                //}
                //else
                //    return false;

                ResourceGroup _ob;

                _ob = (from rs in this.EDMsDataContext.ResourceGroups
                       where rs.Id == ob.Id
                       select rs).First();

                //_ob.Id = ob.Id;
                _ob.Name = ob.Name;
                _ob.Description = ob.Description;
                _ob.LastUpdate = ob.LastUpdate;
                _ob.UpdateBy = ob.UpdateBy;                

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(ResourceGroup ob)
        {
            try
            {
                ResourceGroup _ob = this.GetByID(ob.Id);
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

        /// <summary>
        /// Delete By ID
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(int ID)
        {
            try
            {
                ResourceGroup _ob = this.GetByID(ID);
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
