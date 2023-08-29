

namespace EDMs.Data.DAO.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;
 public    class UsersLoginHistoryDAO :BaseDAO
    {
        public UsersLoginHistoryDAO() : base() { }

        #region GET (Basic)
        public List<UsersLoginHistory> GetAll()
        {
            return this.EDMsDataContext.UsersLoginHistories.ToList<UsersLoginHistory>();
        }

        public UsersLoginHistory GetByID(int ID)
        {
            return this.EDMsDataContext.UsersLoginHistories.FirstOrDefault(ob => ob.ID == ID);
        }

        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets the UsersLoginHistory by UsersLoginHistoryname.
        /// </summary>
        /// <param name="UsersLoginHistoryname">The UsersLoginHistoryname.</param>
        /// <returns></returns>
        public UsersLoginHistory GetUsersLoginHistoryByname(string username , bool ison)
        {
            return this.EDMsDataContext.UsersLoginHistories.FirstOrDefault(ob => ob.UserName == username && ob.IsOn==ison);
        }

    
        #endregion

        #region Insert, Update, Delete
        public bool Insert(UsersLoginHistory ob)
        {
            try
            {
                this.EDMsDataContext.AddToUsersLoginHistories(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

     
        public bool Update(UsersLoginHistory src)
        {
            try
            {
                UsersLoginHistory des;

                des = (from rs in this.EDMsDataContext.UsersLoginHistories
                       where rs.ID == src.ID
                       select rs).First();
                des.UserName = src.UserName;
                des.FullName = src.FullName;
                des.ServerTime = src.ServerTime;
                des.LocalTime = src.LocalTime;
                des.LocalTimeZone = src.LocalTimeZone;
                des.LogoutLocalTime = src.LogoutLocalTime;
                des.DurationTimeLogin = src.DurationTimeLogin;
                des.PhysicalMemory = src.PhysicalMemory;
                des.WindownDomainUser = src.WindownDomainUser;
                des.HostNameComputer = src.HostNameComputer;
                des.Browser = src.Browser;
                des.OSDetail = src.OSDetail;
                des.LanguageFormat = src.LanguageFormat;
                des.IsOn = src.IsOn;
                des.IpAddress = src.IpAddress;

                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(UsersLoginHistory ob)
        {
            try
            {
                UsersLoginHistory _ob = this.GetByID(ob.ID);
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
                UsersLoginHistory _ob = this.GetByID(ID);
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
