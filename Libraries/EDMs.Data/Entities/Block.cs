// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Block.cs" company="">
//   
// </copyright>
// <summary>
//   The block.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace EDMs.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using EDMs.Data.DAO.Library;
    using EDMs.Data.DAO.Security;

    /// <summary>
    /// The block.
    /// </summary>
    public partial class Block
    {
        /// <summary>
        /// Gets the deparment.
        /// </summary>
        public List<Role> ListDeparment
        {
            get
            {
                var roleDao = new RoleDAO();
                var roleIds = this.RoleId.Split(',');

                return roleIds.Select(roleId => roleDao.GetByID(Convert.ToInt32(roleId))).ToList();
            }
        }

        /// <summary>
        /// Gets the project.
        /// </summary>
        public Project Project
        {
            get
            {
                var projectDAO = new ProjectDAO();
                return projectDAO.GetById(this.ProjectId.GetValueOrDefault());
            }
        }
    }
}
