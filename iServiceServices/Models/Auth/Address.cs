﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iServiceServices.Models.Auth
{
    public class Address
    {
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
    }
}