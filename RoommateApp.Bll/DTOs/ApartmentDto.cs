using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Bll.DTOs
{
    public class ApartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int RewardThreshold { get; set; }
        public string ResetSchedule { get; set; } = string.Empty;
        public string InviteCode { get; set; } = string.Empty; // ÚJ
    }
}