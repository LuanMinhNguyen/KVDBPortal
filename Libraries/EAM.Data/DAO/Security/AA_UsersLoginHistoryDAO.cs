

using System.Collections.Generic;
using System.Linq;
using EAM.Data.Entities;

namespace EAM.Data.DAO.Security
{
    public    class AA_UsersLoginHistoryDAO :BaseDAO
    {
        public AA_UsersLoginHistoryDAO() : base() { }

        #region GET (Basic)
        public List<AA_UsersLoginHistory> GetAll()
        {
            return this.EDMsDataContext.AA_UsersLoginHistory.ToList<AA_UsersLoginHistory>();
        }

        public AA_UsersLoginHistory GetByID(int ID)
        {
            return this.EDMsDataContext.AA_UsersLoginHistory.FirstOrDefault(ob => ob.ID == ID);
        }

        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets the AA_UsersLoginHistory by AA_UsersLoginHistoryname.
        /// </summary>
        /// <param name="AA_UsersLoginHistoryname">The AA_UsersLoginHistoryname.</param>
        /// <returns></returns>
        public AA_UsersLoginHistory GetAA_UsersLoginHistoryByname(string username , bool ison)
        {
            return this.EDMsDataContext.AA_UsersLoginHistory.FirstOrDefault(ob => ob.UserName == username && ob.IsOn==ison);
        }

    
        #endregion

        #region Insert, Update, Delete
        public bool Insert(AA_UsersLoginHistory ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_UsersLoginHistory(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

     
        public bool Update(AA_UsersLoginHistory src)
        {
            try
            {
                AA_UsersLoginHistory des;

                des = (from rs in this.EDMsDataContext.AA_UsersLoginHistory
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

        public bool Delete(AA_UsersLoginHistory ob)
        {
            try
            {
                AA_UsersLoginHistory _ob = this.GetByID(ob.ID);
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
                AA_UsersLoginHistory _ob = this.GetByID(ID);
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
