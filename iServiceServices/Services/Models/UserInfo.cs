﻿using iServiceRepositories.Repositories.Models;

namespace iServiceServices.Services.Models
{
    public class UserInfo
    {
        public User User { get; set; }
        public UserRole UserRole { get; set; }
        public UserProfile UserProfile { get; set; }
        public Address? Address { get; set; }
    }
}
