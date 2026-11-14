using Microsoft.EntityFrameworkCore;
using RoommateApp.Db.Entities;

namespace RoommateApp.Db.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Chore> Chores { get; set; }

        public DbSet<Apartment> Apartments { get; set; }

        public DbSet<UserApartment> UserApartments { get; set; } = null!;

        public DbSet<ApartmentCategory> ApartmentCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {  
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserApartment>()
                .HasKey(ua => new { ua.UserId, ua.ApartmentId });

            modelBuilder.Entity<UserApartment>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserApartments)
                .HasForeignKey(ua => ua.UserId);

            modelBuilder.Entity<UserApartment>()
                .HasOne(ua => ua.Apartment)
                .WithMany(a => a.UserApartments)
                .HasForeignKey(ua => ua.ApartmentId);

            modelBuilder.Entity<Apartment>().HasData(
                new Apartment { Id = 1, Name = "Apartment 4B", RewardThreshold = 5000, ResetSchedule = "Sunday 12:00 AM" }
            );

            // Lakótársak kezdőadatai
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Name = "Kovács Péter", Email = "peter@roommate.hu" },
                new User { Id = 2, Name = "Tóth Anna", Email = "anna@roommate.hu" },
                new User { Id = 3, Name = "Szabó Máté", Email = "mate@roommate.hu" }
            );

            // Feladatok (Chores) kezdőadatai az adatbázisban
            modelBuilder.Entity<Chore>().HasData(
    // To Do
    new Chore { Id = 1, Title = "Vacuum living room rug & hallway", Category = "CLEANING", Points = 15, Status = "ToDo", DueDateText = "Today", UserId = 1, ApartmentId = 1 },
    new Chore { Id = 2, Title = "Fix leaky master bath faucet", Category = "MAINTENANCE", Points = 25, Status = "ToDo", DueDateText = "Tomorrow", UserId = 2, ApartmentId = 1 },
    new Chore { Id = 3, Title = "Buy groceries (milk, eggs, fruit)", Category = "SHOPPING", Points = 10, Status = "ToDo", DueDateText = "Oct 24", UserId = 3, ApartmentId = 1 },
    new Chore { Id = 4, Title = "Wipe down all kitchen counters", Category = "CLEANING", Points = 5, Status = "ToDo", DueDateText = "Oct 25", UserId = 2, ApartmentId = 1 },
    new Chore { Id = 5, Title = "Dust bookshelves and tv stand", Category = "CLEANING", Points = 5, Status = "ToDo", DueDateText = "Oct 26", UserId = 1, ApartmentId = 1 },

    // In Progress
    new Chore { Id = 6, Title = "Deep clean kitchen oven & stove", Category = "CLEANING", Points = 20, Status = "InProgress", DueDateText = "Today", UserId = 1, ApartmentId = 1 },
    new Chore { Id = 7, Title = "Replace hallway ceiling bulb", Category = "MAINTENANCE", Points = 10, Status = "InProgress", DueDateText = "Tomorrow", UserId = 2, ApartmentId = 1 },
    new Chore { Id = 8, Title = "Pick up toilet paper and laundry soap", Category = "SHOPPING", Points = 10, Status = "InProgress", DueDateText = "Oct 24", UserId = 3, ApartmentId = 1 },

    // Done
    new Chore { Id = 9, Title = "Take out trash & recycling bins", Category = "CLEANING", Points = 10, Status = "Done", DueDateText = "Done · Oct 21", UserId = 3, ApartmentId = 1 },
    new Chore { Id = 10, Title = "Mop the kitchen tile floors", Category = "CLEANING", Points = 15, Status = "Done", DueDateText = "Done · Oct 22", UserId = 2, ApartmentId = 1 },
    new Chore { Id = 11, Title = "Restock dish soap and hand sanitizer", Category = "SHOPPING", Points = 5, Status = "Done", DueDateText = "Done · Oct 22", UserId = 2, ApartmentId = 1 },
    new Chore { Id = 12, Title = "Tighten loose kitchen cabinet hinge", Category = "MAINTENANCE", Points = 15, Status = "Done", DueDateText = "Done · Oct 23", UserId = 1, ApartmentId = 1 }
);

            modelBuilder.Entity<UserApartment>().HasData(
            new UserApartment { UserId = 1, ApartmentId = 1, IsAdmin = true },  // Kovács Péter (Admin)
            new UserApartment { UserId = 2, ApartmentId = 1, IsAdmin = false }, // Tóth Anna
            new UserApartment { UserId = 3, ApartmentId = 1, IsAdmin = false }  // Szabó Máté
);
        }
    }
}