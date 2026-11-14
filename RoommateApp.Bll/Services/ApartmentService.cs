using Microsoft.EntityFrameworkCore;
using RoommateApp.Bll.DTOs;
using RoommateApp.Db.Context;
using RoommateApp.Db.Entities;

namespace RoommateApp.Bll.Services
{
    public class ApartmentService
    {
        private readonly AppDbContext _context;

        public ApartmentService(AppDbContext context)
        {
            _context = context;
        }

        // Adott szoba lekérése ID alapján
        public async Task<ApartmentDto?> GetApartmentByIdAsync(int apartmentId)
        {
            var apt = await _context.Apartments.FindAsync(apartmentId);
            if (apt == null) return null;

            return new ApartmentDto
            {
                Id = apt.Id,
                Name = apt.Name,
                RewardThreshold = apt.RewardThreshold,
                ResetSchedule = apt.ResetSchedule,
                InviteCode = apt.InviteCode
            };
        }

        // Adott szoba frissítése ID alapján
        public async Task<ApartmentDto?> UpdateApartmentAsync(ApartmentDto dto)
        {
            var apt = await _context.Apartments.FindAsync(dto.Id);
            if (apt == null) return null;

            apt.Name = dto.Name;
            apt.RewardThreshold = dto.RewardThreshold;
            apt.ResetSchedule = dto.ResetSchedule;

            await _context.SaveChangesAsync();

            return new ApartmentDto
            {
                Id = apt.Id,
                Name = apt.Name,
                RewardThreshold = apt.RewardThreshold,
                ResetSchedule = apt.ResetSchedule,
                InviteCode = apt.InviteCode
            };
        }

        public async Task<ApartmentDto> GetApartmentAsync()
        {
            var apt = await _context.Apartments.FirstOrDefaultAsync();
            if (apt == null)
            {
                apt = new Apartment
                {
                    Name = "Budapesti Lakás",
                    RewardThreshold = 5000,
                    ResetSchedule = "Sunday 12:00 AM",
                    InviteCode = "APT4B-8X92"
                };
                _context.Apartments.Add(apt);
                await _context.SaveChangesAsync();
            }

            return new ApartmentDto
            {
                Id = apt.Id,
                Name = apt.Name,
                RewardThreshold = apt.RewardThreshold,
                ResetSchedule = apt.ResetSchedule,
                InviteCode = apt.InviteCode
            };
        }

        public async Task<bool> DeleteApartmentAsync(int id)
        {
            var apartment = await _context.Apartments
                .Include(a => a.UserApartments)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (apartment == null) return false;

            // Töröljük a szobához tartozó chore-okat közvetlenül a táblából
            var choresInApartment = _context.Chores.Where(c => c.ApartmentId == id);
            _context.Chores.RemoveRange(choresInApartment);

            // Töröljük a felhasználó-szoba kapcsolatokat és magát a lakást
            _context.UserApartments.RemoveRange(apartment.UserApartments);
            _context.Apartments.Remove(apartment);

            _context.ApartmentCategories.RemoveRange(_context.ApartmentCategories.Where(c => c.ApartmentId == id));

            await _context.SaveChangesAsync();
            return true;
        }

        // Szobához tartozó kategóriák lekérése (alapértelmezett + adatbázisba mentett egyedi kategóriák)
        public async Task<List<CategoryDto>> GetCategoriesByApartmentAsync(int apartmentId)
        {
            var categories = new List<CategoryDto>
    {
        new() { Name = "CLEANING", ColorHex = "#15803d" },
        new() { Name = "MAINTENANCE", ColorHex = "#be123c" },
        new() { Name = "SHOPPING", ColorHex = "#a16207" }
    };

            var customCategories = await _context.ApartmentCategories
                .Where(c => c.ApartmentId == apartmentId)
                .OrderBy(c => c.Id)
                .ToListAsync();

            foreach (var cat in customCategories)
            {
                if (!categories.Any(c => c.Name.Equals(cat.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    categories.Add(new CategoryDto
                    {
                        Name = cat.Name.ToUpperInvariant(),
                        ColorHex = string.IsNullOrWhiteSpace(cat.ColorHex) ? "#6366f1" : cat.ColorHex
                    });
                }
            }

            return categories;
        }

        public async Task<CategoryDto?> AddCategoryToApartmentAsync(int apartmentId, CategoryDto dto)
        {
            var normalized = dto.Name.Trim().ToUpperInvariant();
            if (string.IsNullOrWhiteSpace(normalized)) return null;

            var color = string.IsNullOrWhiteSpace(dto.ColorHex) ? "#6366f1" : dto.ColorHex.Trim();

            var defaults = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["CLEANING"] = "#15803d",
                ["MAINTENANCE"] = "#be123c",
                ["SHOPPING"] = "#a16207"
            };

            if (defaults.TryGetValue(normalized, out var defaultColor))
            {
                return new CategoryDto { Name = normalized, ColorHex = defaultColor };
            }

            var existing = await _context.ApartmentCategories
                .FirstOrDefaultAsync(c => c.ApartmentId == apartmentId && c.Name == normalized);

            if (existing == null)
            {
                _context.ApartmentCategories.Add(new ApartmentCategory
                {
                    ApartmentId = apartmentId,
                    Name = normalized,
                    ColorHex = color
                });
                await _context.SaveChangesAsync();
            }
            else
            {
                existing.ColorHex = color;
                await _context.SaveChangesAsync();
            }

            return new CategoryDto { Name = normalized, ColorHex = color };
        }
    }
}