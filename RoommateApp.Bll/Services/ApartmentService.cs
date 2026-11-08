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
    }
}