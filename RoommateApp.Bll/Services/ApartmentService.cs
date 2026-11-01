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

        public async Task<ApartmentDto> GetApartmentAsync()
        {
            var apt = await _context.Apartments.FirstOrDefaultAsync();
            if (apt == null)
            {
                apt = new Apartment { Name = "Apartment 4B", RewardThreshold = 5000, ResetSchedule = "Sunday 12:00 AM" };
                _context.Apartments.Add(apt);
                await _context.SaveChangesAsync();
            }

            return new ApartmentDto
            {
                Id = apt.Id,
                Name = apt.Name,
                RewardThreshold = apt.RewardThreshold,
                ResetSchedule = apt.ResetSchedule
            };
        }

        public async Task<ApartmentDto> UpdateApartmentAsync(ApartmentDto dto)
        {
            var apt = await _context.Apartments.FirstOrDefaultAsync();
            if (apt == null)
            {
                apt = new Apartment();
                _context.Apartments.Add(apt);
            }

            apt.Name = dto.Name;
            apt.RewardThreshold = dto.RewardThreshold;
            apt.ResetSchedule = dto.ResetSchedule;

            await _context.SaveChangesAsync(); 

            return new ApartmentDto
            {
                Id = apt.Id,
                Name = apt.Name,
                RewardThreshold = apt.RewardThreshold,
                ResetSchedule = apt.ResetSchedule
            };
        }
    }
}