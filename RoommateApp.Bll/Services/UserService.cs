using Microsoft.EntityFrameworkCore;
using RoommateApp.Bll.DTOs;
using RoommateApp.Db;
using RoommateApp.Db.Context;
using RoommateApp.Db.Entities;

namespace RoommateApp.Bll.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        // Egy adott szoba lakóinak lekérése
        public async Task<List<UserDto>> GetUsersByApartmentAsync(int apartmentId)
        {
            return await _context.UserApartments
                .Where(ua => ua.ApartmentId == apartmentId)
                .Select(ua => new UserDto
                {
                    Id = ua.User.Id,
                    Name = ua.User.Name,
                    Email = ua.User.Email,
                    IsAdmin = ua.IsAdmin
                })
                .ToListAsync();
        }

        // A felhasználó szobáinak lekérése
        public async Task<List<ApartmentDto>> GetUserApartmentsAsync(int userId)
        {
            return await _context.UserApartments
                .Where(ua => ua.UserId == userId)
                .Select(ua => new ApartmentDto
                {
                    Id = ua.Apartment.Id,
                    Name = ua.Apartment.Name,
                    RewardThreshold = ua.Apartment.RewardThreshold,
                    ResetSchedule = ua.Apartment.ResetSchedule,
                    InviteCode = ua.Apartment.InviteCode
                })
                .ToListAsync();
        }

        // Új szoba létrehozása (a létrehozó Admin lesz ebben a szobában)
        public async Task<ApartmentDto> CreateApartmentForUserAsync(int userId, string roomName)
        {
            var apartment = new Apartment
            {
                Name = roomName,
                RewardThreshold = 5000,
                ResetSchedule = "Sunday 12:00 AM",
                InviteCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper()
            };

            _context.Apartments.Add(apartment);
            await _context.SaveChangesAsync();

            var userApartment = new UserApartment
            {
                UserId = userId,
                ApartmentId = apartment.Id,
                IsAdmin = true // Létrehozó = Admin
            };

            _context.UserApartments.Add(userApartment);
            await _context.SaveChangesAsync();

            return new ApartmentDto
            {
                Id = apartment.Id,
                Name = apartment.Name,
                RewardThreshold = apartment.RewardThreshold,
                ResetSchedule = apartment.ResetSchedule,
                InviteCode = apartment.InviteCode
            };
        }

        // Csatlakozás szobához kód alapján
        public async Task<bool> JoinApartmentByCodeAsync(int userId, string inviteCode)
        {
            var apartment = await _context.Apartments.FirstOrDefaultAsync(a => a.InviteCode == inviteCode);
            if (apartment == null) return false;

            bool alreadyInRoom = await _context.UserApartments
                .AnyAsync(ua => ua.UserId == userId && ua.ApartmentId == apartment.Id);

            if (alreadyInRoom) return true;

            _context.UserApartments.Add(new UserApartment
            {
                UserId = userId,
                ApartmentId = apartment.Id,
                IsAdmin = false
            });

            await _context.SaveChangesAsync();
            return true;
        }

        // Eltávolítás az adott szobából
        public async Task<bool> RemoveUserFromApartmentAsync(int userId, int apartmentId)
        {
            var ua = await _context.UserApartments
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ApartmentId == apartmentId);

            if (ua == null) return false;

            _context.UserApartments.Remove(ua);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<UserDto> GetOrCreateGoogleUserAsync(string email, string name)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (existingUser != null)
            {
                return new UserDto
                {
                    Id = existingUser.Id,
                    Name = existingUser.Name,
                    Email = existingUser.Email
                };
            }

            var newUser = new User
            {
                Name = string.IsNullOrWhiteSpace(name) ? email.Split('@')[0] : name,
                Email = email,
                Password = "GOOGLE_AUTH_EXTERNAL"
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = newUser.Id,
                Name = newUser.Name,
                Email = newUser.Email
            };
        }
    }
}