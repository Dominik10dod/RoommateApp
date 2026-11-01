using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Db.Entities
{
    public class UserApartment
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; } = null!;

        // Szobánkénti jogosultság! (Az egyik szobában lehetsz Admin, a másikban Member)
        public bool IsAdmin { get; set; } = false;
    }
}