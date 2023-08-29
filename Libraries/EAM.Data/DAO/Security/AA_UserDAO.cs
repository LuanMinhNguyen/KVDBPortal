using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EAM.Data.Entities;

namespace EAM.Data.DAO.Security
{
    public class AA_UsersDAO : BaseDAO
    {
        public AA_UsersDAO() : base() { }

        #region GET (Basic)
        public List<AA_Users> GetAll()
        {
            return this.EDMsDataContext.AA_Users.ToList<AA_Users>();
        }

        public AA_Users GetByID(int ID)
        {
            return this.EDMsDataContext.AA_Users.FirstOrDefault(ob => ob.Id == ID);
        }
       
        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets the AA_Users by AA_Usersname.
        /// </summary>
        /// <param name="AA_Usersname">The AA_Usersname.</param>
        /// <returns></returns>
        public AA_Users GetAA_UsersByAA_Usersname(string Username)
        {
            return this.EDMsDataContext.AA_Users.FirstOrDefault(ob => ob.Username == Username);
        }

        /// <summary>
        /// The get all by role id.
        /// </summary>
        /// <param name="roleId">
        /// The role id.
        /// </param>
        /// <returns>
        /// The <see cref="List"/>.
        /// </returns>
        public List<AA_Users>  GetAllByRoleId(int roleId)
        {
            return this.EDMsDataContext.AA_Users.Where(t => t.RoleId == roleId).ToList();
        }

        public List<AA_Users> GetSpecialListAA_Users(List<int> roleIds)
        {
            return
                this.EDMsDataContext.AA_Users.ToArray().Where(t => roleIds.Contains(t.RoleId.GetValueOrDefault())).ToList();
        } 

        /// <summary>
        /// Gets the by resource id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public AA_Users GetByResourceId(int id)
        {
            return this.EDMsDataContext.AA_Users.FirstOrDefault(x => x.ResourceId == id);
        }

        #endregion

        #region Insert, Update, Delete
        public bool Insert(AA_Users ob)
        {
            try
            {
                this.EDMsDataContext.AddToAA_Users(ob);
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="AA_UsersId">The AA_Users id.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public bool ChangePassword(int AA_UsersId, string newPassword)
        {
            var AA_Users = (from item in this.EDMsDataContext.AA_Users
                    where item.Id == AA_UsersId
                    select item).FirstOrDefault();
            if(AA_Users != null)
            {
                AA_Users.Password = newPassword;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(AA_Users src)
        {
            try
            {
                AA_Users des;

                des = (from rs in this.EDMsDataContext.AA_Users
                       where rs.Id == src.Id
                       select rs).First();

                des.RoleId = src.RoleId;
                des.Username = src.Username;
                //_ob.Password = ob.Password;
                des.Email = src.Email;
                des.Employee_Ref = src.Employee_Ref;
                des.Status = src.Status;
                des.Active = src.Active;
                des.FullName = src.FullName;
                des.HashCode = src.HashCode;
                des.Position = src.Position;
                des.Phone = src.Phone;
                des.CellPhone = src.CellPhone;
                des.TitleName = src.TitleName;
                des.TitleId = src.TitleId;
                des.IsActive = src.IsActive;
                des.CommentGroupId = src.CommentGroupId;
                des.CommentGroupName = src.CommentGroupName;
                des.RoleName = src.RoleName;
                des.LocationId = src.LocationId;
                des.LocationName = src.LocationName;
                des.ProjectId = src.ProjectId;
                des.ProjectName = src.ProjectName;
                des.IsDeptManager = src.IsDeptManager;
                des.IsDC = src.IsDC;
                des.ConfidentialId = src.ConfidentialId;
                des.ConfidentialName = src.ConfidentialName;
                des.IsSendMail = src.IsSendMail;
                des.IsPD = src.IsPD;
                des.OrgCode = src.OrgCode;
                des.IsTTYT = src.IsTTYT;
                des.SoTYT = src.SoTYT;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(AA_Users ob)
        {
            try
            {
                AA_Users _ob = this.GetByID(ob.Id);
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
                AA_Users _ob = this.GetByID(ID);
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

        /// <summary>
        /// Checks the exists.
        /// </summary>
        /// <param name="AA_UsersId">The AA_Users id.</param>
        /// <param name="AA_UsersName">Name of the AA_Users.</param>
        /// <returns></returns>
        public bool CheckExists(int? AA_UsersId, string AA_UsersName)
        {
            if(AA_UsersId == null)
            {
                return this.EDMsDataContext.AA_Users.Any(x => x.Username == AA_UsersName);
            }
            return this.EDMsDataContext.AA_Users.Any(x => x.Username == AA_UsersName && x.Id != AA_UsersId.Value);
        }
    }
}
