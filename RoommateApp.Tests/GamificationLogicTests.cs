using RoommateApp.Web.Client;
using Xunit;

namespace RoommateApp.Tests
{
    public class GamificationLogicTests
    {
        [Fact]
        public void LeaderboardRanking_ShouldCountOnlyDoneChores_AndOrderByPointsDescending()
        {
            // Arrange
            var users = new List<UserDto>
            {
                new() { Id = 1, Name = "Kovács Péter" },
                new() { Id = 2, Name = "Tóth Anna" },
                new() { Id = 3, Name = "Mátyás Dominik" }
            };

            var chores = new List<ChoreDto>
            {
                new() { Id = 1, Title = "Porszívózás", Status = "Done", Points = 30, UserId = 1 },
                new() { Id = 2, Title = "Mosogatás", Status = "ToDo", Points = 50, UserId = 1 },       // Nem Done -> nem számít
                new() { Id = 3, Title = "Ablakpucolás", Status = "Done", Points = 90, UserId = 3 },
                new() { Id = 4, Title = "Bevásárlás", Status = "InProgress", Points = 40, UserId = 2 }, // Nem Done -> nem számít
                new() { Id = 5, Title = "Szemétlevitel", Status = "Done", Points = 20, UserId = 2 }
            };

            // Act
            var leaderboard = users
                .Select(u => new
                {
                    User = u,
                    Points = chores
                        .Where(c => c.UserId == u.Id && c.Status == "Done")
                        .Sum(c => c.Points)
                })
                .OrderByDescending(x => x.Points)
                .ToList();

            // Assert
            Assert.Equal(3, leaderboard.Count);
            Assert.Equal("Mátyás Dominik", leaderboard[0].User.Name);
            Assert.Equal(90, leaderboard[0].Points);

            Assert.Equal("Kovács Péter", leaderboard[1].User.Name);
            Assert.Equal(30, leaderboard[1].Points);

            Assert.Equal("Tóth Anna", leaderboard[2].User.Name);
            Assert.Equal(20, leaderboard[2].Points);
        }

        [Theory]
        [InlineData(130, 5000, 2)]
        [InlineData(2500, 5000, 50)]
        [InlineData(6500, 5000, 100)] // 100%-nál maximálva van
        [InlineData(100, 0, 0)]       // 0 küszöb esetén nincs nullával osztás
        public void ApartmentRewardProgress_ShouldCalculatePercentageSafely(int totalPoints, int threshold, int expectedProgress)
        {
            // Act
            int progress = threshold > 0
                ? Math.Min(100, (totalPoints * 100) / threshold)
                : 0;

            // Assert
            Assert.Equal(expectedProgress, progress);
        }
    }
}