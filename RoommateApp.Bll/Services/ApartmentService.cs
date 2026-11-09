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

            await _context.SaveChangesAsync();
            return true;
        }
    }
}