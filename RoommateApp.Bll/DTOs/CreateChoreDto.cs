using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Bll.DTOs
{
    public class CreateChoreDto
    {
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Points { get; set; }
        public string DueDateText { get; set; } = "Today";
        public int UserId { get; set; }
        public int ApartmentId { get; set; } 
    }
}