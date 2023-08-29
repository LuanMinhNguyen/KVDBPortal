using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EAM.XNKTWebAPICore.Models
{
    public class FileInformation
    {
        public IFormFile file { get; set; }
    }
}
