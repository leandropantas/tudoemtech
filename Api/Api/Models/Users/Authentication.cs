using System;

namespace SoftTracerAPI.Models
{
    public class Authentication
    {

        public string UserId { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public Guid Token { get; set; }

    }
}