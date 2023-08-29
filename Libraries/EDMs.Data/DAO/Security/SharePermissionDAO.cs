namespace EDMs.Data.DAO.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class SharePermissionDAO : BaseDAO
    {
        public SharePermissionDAO() : base() { }

        #region GET (Basic)
        public IQueryable<SharePermission> GetIQueryable()
        {
            return this.EDMsDataContext.SharePermissions;
        }

        public List<SharePermission> GetAll()
        {
            return this.EDMsDataContext.SharePermissions.ToList();
        }

        public SharePermission GetById(Guid id)
        {
            return this.EDMsDataContext.SharePermissions.FirstOrDefault(ob => ob.ID == id);
        }
       
        #endregion

        #region Get (Advances)

        public List<SharePermission> GetByFromUser(int userId)
        {
            return this.EDMsDataContext.SharePermissions.Where(ob => ob.FromUserId == userId).ToList();
        }

        #endregion

        #region Insert, Update, Delete

        /// <summary>
        /// Deletes the SharePermissions.
        /// </summary>
        /// <param name="SharePermissions">The SharePermissions.</param>
        /// <returns></returns>
        public bool DeleteSharePermissions(List<SharePermission> SharePermissions)
        {
            try
            {
                foreach (var item in SharePermissions)
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
        /// Adds the SharePermissions.
        /// </summary>
        /// <param name="SharePermissions">The SharePermissions.</param>
        /// <returns></returns>
        public bool AddSharePermissions(List<SharePermission> SharePermissions)
        {
            try
            {
                foreach (var item in SharePermissions)
                {
                    this.EDMsDataContext.AddToSharePermissions(item);
                    this.EDMsDataContext.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(SharePermission src)
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
        public bool Delete(Guid id)
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


        public Guid? Insert(SharePermission ob)
        {
            try
            {
                this.EDMsDataContext.AddToSharePermissions(ob);
                this.EDMsDataContext.SaveChanges();
                return ob.ID;
            }
            catch
            {
                return null;
            }
        }

        public bool Update(SharePermission ob)
        {
            try
            {
                SharePermission _ob;

                _ob = (from rs in this.EDMsDataContext.SharePermissions
                       where rs.ID == ob.ID
                       select rs).First();

                _ob.ObjectList = ob.ObjectList;
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
