using iServiceRepositories.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iServiceServices.Services.Models
{
    public class ProfileUpdate
    {
        public string Name { get; set; }
        public ClientProfile? ClientProfile { get; set; }
        public EstablishmentProfile? EstablishmentProfile { get; set; }
    }
}
