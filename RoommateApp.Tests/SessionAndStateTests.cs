using RoommateApp.Web.Client;
using RoommateApp.Web.Client.Services;
using Xunit;

namespace RoommateApp.Tests
{
    public class SessionAndStateTests
    {
        [Fact]
        public void SetUser_ShouldMarkUserAsLoggedIn_AndTriggerOnChangeEvent()
        {
            // Arrange
            var session = new UserSessionService();
            bool eventTriggered = false;
            session.OnChange += () => eventTriggered = true;

            var user = new UserDto
            {
                Id = 1,
                Name = "Mátyás Dominik",
                Email = "dominik@example.com"
            };

            // Act
            session.SetUser(user);

            // Assert
            Assert.True(session.IsLoggedIn);
            Assert.NotNull(session.CurrentUser);
            Assert.Equal("Mátyás Dominik", session.CurrentUser!.Name);
            Assert.True(eventTriggered);
        }

        [Fact]
        public void Logout_ShouldClearCurrentUser_AndResetLoginState()
        {
            // Arrange
            var session = new UserSessionService();
            session.SetUser(new UserDto { Id = 2, Name = "Tóth Anna" });

            // Act
            session.Logout();

            // Assert
            Assert.False(session.IsLoggedIn);
            Assert.Null(session.CurrentUser);
        }

        [Fact]
        public void NotifyPointsChanged_ShouldInvokeOnPointsChangedSubscribers()
        {
            // Arrange
            var session = new UserSessionService();
            int triggerCount = 0;
            session.OnPointsChanged += () => triggerCount++;

            // Act
            session.NotifyPointsChanged();
            session.NotifyPointsChanged();

            // Assert
            Assert.Equal(2, triggerCount);
        }
    }
}