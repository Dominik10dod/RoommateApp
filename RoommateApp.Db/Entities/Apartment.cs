using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Db.Entities
{
    public class Apartment
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RewardThreshold { get; set; } = 5000;
        public string ResetSchedule { get; set; } = "Weekly";
        public DateTime? LastResetDate { get; set; }
        public string InviteCode { get; set; } = string.Empty;

        // Szoba lakói
        public ICollection<UserApartment> UserApartments { get; set; } = new List<UserApartment>();
    }
}