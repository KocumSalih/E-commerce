﻿using System;

namespace ECommerceProjectWithWebAPI.Entities.Dtos.UserDtos
{
    using Abstract;

    public class UserDto:IDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}
