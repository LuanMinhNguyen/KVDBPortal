using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EAM.LDAWebAPICore.Models
{
    public class UserInforDto
    {
        public string UserName { get; set; }
        public bool IsDelete { get; set; }

    }
}
