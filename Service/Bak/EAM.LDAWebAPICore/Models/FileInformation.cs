using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace EAM.LDAWebAPICore.Models
{
    public class FileInformation
    {
        public IFormFile file { get; set; }
    }
}
