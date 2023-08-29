using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EDMs.Data.Entities
{
    using EDMs.Data.DAO.Library;

    public partial class Field
    {
        public Block Block
        {
            get
            {
                var blockDAO = new BlockDAO();
                return blockDAO.GetById(this.BlockId.GetValueOrDefault());
            }
        }
    }
}
