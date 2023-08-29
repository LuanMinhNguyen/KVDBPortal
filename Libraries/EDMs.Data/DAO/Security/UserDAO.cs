using System.Security.Cryptography;

namespace EDMs.Data.DAO.Security
{
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.Entities;

    public class UserDAO : BaseDAO
    {
        public UserDAO() : base() { }

        #region GET (Basic)
        public List<User> GetAll()
        {
            return this.EDMsDataContext.Users.ToList<User>();
        }

        public User GetByID(int ID)
        {
            return this.EDMsDataContext.Users.FirstOrDefault(ob => ob.Id == ID);
        }
       
        #endregion

        #region Get (Advances)

        /// <summary>
        /// Gets the user by username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public User GetUserByUsername(string username)
        {
            return this.EDMsDataContext.Users.FirstOrDefault(ob => ob.Username == username);
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
        public List<User>  GetAllByRoleId(int roleId)
        {
            return this.EDMsDataContext.Users.Where(t => t.RoleId == roleId).ToList();
        }

        public List<User> GetSpecialListUser(List<int> roleIds)
        {
            return
                this.EDMsDataContext.Users.ToArray().Where(t => roleIds.Contains(t.RoleId.GetValueOrDefault())).ToList();
        } 

        /// <summary>
        /// Gets the by resource id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public User GetByResourceId(int id)
        {
            return this.EDMsDataContext.Users.FirstOrDefault(x => x.ResourceId == id);
        }

        #endregion

        #region Insert, Update, Delete
        public bool Insert(User ob)
        {
            try
            {
                this.EDMsDataContext.AddToUsers(ob);
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
        /// <param name="userId">The user id.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public bool ChangePassword(int userId, string newPassword)
        {
            var user = (from item in this.EDMsDataContext.Users
                    where item.Id == userId
                    select item).FirstOrDefault();
            if(user != null)
            {
                user.Password = newPassword;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            return false;
        }

        public bool Update(User src)
        {
            try
            {
                User des;

                des = (from rs in this.EDMsDataContext.Users
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
                des.ManagerId = src.ManagerId;
                des.ManagerName = src.ManagerName;
                des.ManagerIds = src.ManagerIds;
                this.EDMsDataContext.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(User ob)
        {
            try
            {
                User _ob = this.GetByID(ob.Id);
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
                User _ob = this.GetByID(ID);
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
        /// <param name="userId">The user id.</param>
        /// <param name="userName">Name of the user.</param>
        /// <returns></returns>
        public bool CheckExists(int? userId, string userName)
        {
            if(userId == null)
            {
                return this.EDMsDataContext.Users.Any(x => x.Username == userName);
            }
            return this.EDMsDataContext.Users.Any(x => x.Username == userName && x.Id != userId.Value);
        }
    }
}
