using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iServiceServices.Services.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public byte[]? Photo { get; set; }
    }
}
