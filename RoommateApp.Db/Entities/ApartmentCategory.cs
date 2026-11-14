using System;
using System.Collections.Generic;
using System.Text;

namespace RoommateApp.Db.Entities
{
    public class ApartmentCategory
    {
        public int Id { get; set; }
        public int ApartmentId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ColorHex { get; set; } = "#6366f1";

        public Apartment Apartment { get; set; } = null!;
    }
}