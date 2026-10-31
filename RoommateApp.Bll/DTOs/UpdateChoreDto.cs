using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Bll.DTOs
{
    public class UpdateChoreDto
    {
        public int Id { get; set; }
        public string Status { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string DueDateText { get; set; } = string.Empty;
    }
}