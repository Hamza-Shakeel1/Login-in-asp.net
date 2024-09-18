using System;
using System.Collections.Generic;

namespace Logins.Models
{
    public partial class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int? Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Picture { get; set; }
    }
}
