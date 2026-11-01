using Microsoft.EntityFrameworkCore;
using RoommateApp.Bll.DTOs;
using RoommateApp.Db.Context;
using RoommateApp.Db.Entities;

namespace RoommateApp.Bll.Services
{
    public class ChoreService
    {
        private readonly AppDbContext _context;

        public ChoreService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ChoreDto>> GetChoresAsync()
        {
            return await _context.Chores
                .Include(c => c.User)
                .Select(c => new ChoreDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Category = c.Category,
                    Points = c.Points,
                    Status = c.Status,
                    DueDateText = c.DueDateText,
                    UserId = c.UserId,
                    UserName = c.User != null ? c.User.Name : "Ismeretlen",
                    UserAvatarUrl = $"https://i.pravatar.cc/150?img={c.UserId + 10}"
                })
                .ToListAsync();
        }

        // ÚJ: Elérhető kategóriák dinamikus kigyűjtése az adatbázisból
        public async Task<List<string>> GetCategoriesAsync()
        {
            var defaults = new List<string> { "CLEANING", "MAINTENANCE", "SHOPPING" };
            var dbCategories = await _context.Chores
                .Select(c => c.Category.ToUpper())
                .Distinct()
                .ToListAsync();

            return defaults.Union(dbCategories).ToList();
        }

        // ÚJ: Új feladat létrehozása az adatbázisban "ToDo" státusszal
        public async Task<ChoreDto> CreateChoreAsync(CreateChoreDto dto)
        {
            var chore = new Chore
            {
                Title = dto.Title,
                Category = dto.Category.ToUpper(),
                Points = dto.Points,
                Status = "ToDo", 
                DueDateText = string.IsNullOrWhiteSpace(dto.DueDateText) ? "Today" : dto.DueDateText,
                UserId = dto.UserId > 0 ? dto.UserId : 1
            };

            _context.Chores.Add(chore);
            await _context.SaveChangesAsync(); 

            var user = await _context.Users.FindAsync(chore.UserId);

            return new ChoreDto
            {
                Id = chore.Id,
                Title = chore.Title,
                Category = chore.Category,
                Points = chore.Points,
                Status = chore.Status,
                DueDateText = chore.DueDateText,
                UserId = chore.UserId,
                UserName = user?.Name ?? "Ismeretlen",
                UserAvatarUrl = $"https://i.pravatar.cc/150?img={chore.UserId + 10}"
            };
        }

        public async Task<bool> UpdateChoreStatusAsync(UpdateChoreDto dto)
        {
            var chore = await _context.Chores.FindAsync(dto.Id);
            if (chore == null) return false;

            // Adatbázis mezők frissítése
            chore.Status = dto.Status;
            chore.UserId = dto.UserId;
            if (!string.IsNullOrEmpty(dto.DueDateText))
            {
                chore.DueDateText = dto.DueDateText;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}