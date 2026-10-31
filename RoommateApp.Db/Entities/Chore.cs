using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Db.Entities
{
    public class Chore
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty; // CLEANING, MAINTENANCE, SHOPPING
        public int Points { get; set; }
        public string Status { get; set; } = string.Empty; // ToDo, InProgress, Done
        public string DueDateText { get; set; } = string.Empty; // "Today", "Tomorrow", "Oct 24", "Done · Oct 21"

        // Lakótárs / Felhasználó kapcsolat
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}