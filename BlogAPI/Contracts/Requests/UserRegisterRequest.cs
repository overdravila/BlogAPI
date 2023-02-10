﻿namespace BlogAPI.Contracts.Requests
{
    public class UserRegisterRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PersonType { get; set; }

    }
}
