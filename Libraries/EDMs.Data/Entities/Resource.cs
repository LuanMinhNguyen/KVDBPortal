using EDMs.Data.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;
    using EDMs.Data.DAO.Security;

    public partial class Resource
    {
        public ResourceGroup ResourceGroup {
            get 
            { 
                ResourceGroupDAO dao = new ResourceGroupDAO();
                return dao.GetByID(this.ResourceGroupId.Value);
            }
        }

        public User User
        {
            get
            {
                var dao = new UserDAO();
                return dao.GetByResourceId(Id);
            }
        }
    }
}
