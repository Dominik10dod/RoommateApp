using Microsoft.EntityFrameworkCore;
using RoommateApp.Bll.DTOs;
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

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserDto { Id = u.Id, Name = u.Name, Email = u.Email, IsAdmin = u.IsAdmin })
                .ToListAsync();
        }

        // ÚJ: Új felhasználó regisztrálása és lementése az adatbázisba
        public async Task<UserDto?> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return null; // Már létezik ilyen email

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
        }

        // ÚJ: Bejelentkezés ellenőrzése
        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email && u.Password == dto.Password);

            if (user == null) return null;

            return new UserDto { Id = user.Id, Name = user.Name, Email = user.Email };
        }
    }
}