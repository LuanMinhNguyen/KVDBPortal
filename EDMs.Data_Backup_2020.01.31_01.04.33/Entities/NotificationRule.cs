using EDMs.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;

    public partial class NotificationRule
    {
        /// <summary>
        /// Gets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Discipline Discipline
        {
            get
            {
                var dao = new DisciplineDAO();
                return dao.GetById(this.DisciplineID.GetValueOrDefault());
            }
        }
    }
}
