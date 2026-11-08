namespace RoommateApp.Db.Entities
{
    public class Chore
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int Points { get; set; }
        public string Status { get; set; } = string.Empty;
        public string DueDateText { get; set; } = string.Empty;

        // Lakótárs kapcsolat
        public int UserId { get; set; }
        public User? User { get; set; }

        // ÚJ: Szoba kapcsolat (így a feladat az adott szobához tartozik)
        public int ApartmentId { get; set; }
        public Apartment? Apartment { get; set; }
    }
}