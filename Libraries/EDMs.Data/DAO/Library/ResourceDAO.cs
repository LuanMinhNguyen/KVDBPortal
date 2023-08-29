namespace EDMs.Data.DAO.Library
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class ResourceDAO : BaseDAO
    {
        public ResourceDAO() : base() { }

        #region GET (Basic)
        public IQueryable<Resource> GetIQueryable()
        {
            return this.EDMsDataContext.Resources;
        }

        public IQueryable<Resource> GetAll(int pageSize, int startingRecordNumber)
        {
            return this.EDMsDataContext.Resources.OrderBy(t => t.Email).Skip(startingRecordNumber).Take(pageSize);
        }

        public List<Resource> GetAll()
        {
            return this.EDMsDataContext.Resources.ToList();
        }

        public int GetItemCount()
        {
            return this.EDMsDataContext.Resources.Count();
        }

        public List<Resource> GetAllIsResource()
        {
            return this.EDMsDataContext.Resources.Where(ob => ob.IsResource == true).ToList<Resource>();
        }
        public Resource GetByID(int ID)
        {
            return this.EDMsDataContext.Resources.Where(ob => ob.Id == ID).FirstOrDefault<Resource>();
        }
       
        #endregion

        #region GET ADVANCE

        public List<Resource> GetByResourceGroup(int resourceGroupId)
        {
            return this.EDMsDataContext.Resources.ToList().Where(ob => ob.ResourceGroupId == resourceGroupId && ob.IsResource == true).ToList<Resource>();
        }

        public List<Resource> GetByFullName(string fullName)
        {
            return this.EDMsDataContext.Resources.Where(ob => ob.FullName.Contains(fullName)).ToList<Resource>();
        }
        public List<Resource> GetByFullNameIsResource(string fullName)
        {
            return this.EDMsDataContext.Resources.Where(ob => ob.FullName.Contains(fullName) && ob.IsResource == true).ToList<Resource>();
        }
        public List<Resource> GetByFullName(string fullName,int resourceGroupId)
        {
            return this.EDMsDataContext.Resources.ToList().Where(ob => ob.FullName.Contains(fullName) && ob.ResourceGroupId == resourceGroupId).ToList<Resource>();
        }
        public List<Resource> GetByFullNameIsResource(string fullName, int resourceGroupId)
        {
            return this.EDMsDataContext.Resources.ToList().Where(ob => ob.FullName.Contains(fullName) && ob.ResourceGroupId == resourceGroupId && ob.IsResource == true).ToList<Resource>();
        }
        public List<Resource> GetByIsResource(bool isResource)
        {
            return this.EDMsDataContext.Resources.ToList().Where(ob => ob.IsResource == isResource).ToList<Resource>();
        }
        #endregion

        #region Insert, Update, Delete
        public int? Insert(Resource ob)
        {
            try
            {
                this.EDMsDataContext.AddToResources(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.Id;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(Resource ob)
        {
            try
            {
                //Resource _ob = GetByID(ob.Id);
                //if (_ob != null)
                //{
                //    EDMsDataContext.ApplyCurrentValues(_ob.EntityKey.EntitySetName, ob);
                //    EDMsDataContext.SaveChanges();
                //    return true;
                //}
                //else
                //    return false;

                Resource _ob;

                _ob = (from rs in this.EDMsDataContext.Resources
                       where rs.Id == ob.Id
                       select rs).First();

                //_ob.Id = ob.Id;
                _ob.Address1 = ob.Address1;
                _ob.Address2 = ob.Address2;
                _ob.CellPhone = ob.CellPhone;
                _ob.City = ob.City;
                _ob.Color = ob.Color;
                _ob.CssClass = ob.CssClass;
                _ob.DateOfBirth = ob.DateOfBirth;
                _ob.Email = ob.Email;
                _ob.FirstName = ob.FirstName;
                _ob.FullName = ob.FullName;
                _ob.Generation = ob.Generation;
                _ob.HomePhone = ob.HomePhone;
                _ob.IdentityCard = ob.IdentityCard;
                _ob.LastName = ob.LastName;
                _ob.MailLabel = ob.MailLabel;
                _ob.MarialStatus = ob.MarialStatus;
                _ob.MiddleName = ob.MiddleName;
                _ob.Occupation = ob.Occupation;
                _ob.Sex = ob.Sex;
                _ob.SSN = ob.SSN;
                _ob.StateCode = ob.StateCode;
                _ob.StateName = ob.StateName;
                _ob.WorkPhone = ob.WorkPhone;
                _ob.ZipCode = ob.ZipCode;
                _ob.IsResource = ob.IsResource;
                _ob.ResourceGroupId = ob.ResourceGroupId;
                _ob.IsFulltime = ob.IsFulltime;

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

        public bool Delete(Resource ob)
        {
            try
            {
                Resource _ob = this.GetByID(ob.Id);
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
                Resource _ob = this.GetByID(ID);
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
