using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Db.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Csak ez a gyűjtemény maradjon!
        public ICollection<UserApartment> UserApartments { get; set; } = new List<UserApartment>();
    }
}