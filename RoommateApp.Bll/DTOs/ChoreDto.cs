using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Bll.DTOs
{
    public class ChoreDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Points { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DueDateText { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserAvatarUrl { get; set; } = string.Empty;
    }
}