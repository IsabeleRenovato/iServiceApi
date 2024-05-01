using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iServiceServices.Services.Models
{
    public class ImageModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public IFormFile File { get; set; }
    }
}
