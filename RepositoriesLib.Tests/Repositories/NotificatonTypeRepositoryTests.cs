using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;
using Xunit;

namespace RepositoriesLib.Tests.Repositories
{
    public class NotificatonTypeRepositoryTests
    {
        private readonly IMockHelper<INotificationTypeRepository> _mockHelper;
        private readonly INotificationTypeRepository _notificationTypeRepository;
        public NotificatonTypeRepositoryTests() {
            _mockHelper = new NotificationTypeMockHelper();
            _notificationTypeRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task RetrieveAllAsync_Should_ReturnAllNotificationTypes()
        {
            var actualNotificationTypes = await _notificationTypeRepository.RetrieveAllAsync();

            Assert.NotNull(actualNotificationTypes);
            Assert.Equal(10, actualNotificationTypes.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingNotificationTypeId_Should_ReturnNotificationType()
        {
            
            var notificationTypeId = 2;

            
            var actualNotificationType = await _notificationTypeRepository.RetrieveAsync(notificationTypeId);

            
            Assert.NotNull(actualNotificationType);
            Assert.Equal(2, actualNotificationType.Id);
            Assert.Equal($"Notification Type - {notificationTypeId}", actualNotificationType.Title);
            Assert.Equal($"Description for Notification Type - {notificationTypeId}", actualNotificationType.Description);
        }

        [Fact]
        public async Task CreateAsync_Should_AddNotificationTypeToRepository()
        {
            
            var newNotificationType = new NotificationType
            {
                Title = "New Notification Type",
                Description = "Description for New Notification Type"
            };

           
            var createdNotificationType = await _notificationTypeRepository.CreateAsync(newNotificationType);

            
            Assert.NotNull(createdNotificationType);
            Assert.Equal(newNotificationType.Title, createdNotificationType.Title);
            Assert.Equal(newNotificationType.Description, createdNotificationType.Description);
            Assert.Equal(11, createdNotificationType.Id);
        }

        [Fact]
        public async Task UpdateAsync_ExistingNotificationTypeId_Should_UpdateNotificationType()
        {
            
            var notificationTypeId = 3;
            var existingNotificationType = await _notificationTypeRepository.RetrieveAsync(notificationTypeId);

            Assert.NotNull(existingNotificationType);
            existingNotificationType.Title = "Updated Title";

            var updatedNotificationType = await _notificationTypeRepository.UpdateAsync(notificationTypeId, existingNotificationType);

            
            Assert.NotNull(updatedNotificationType);
            Assert.Equal(existingNotificationType.Title, updatedNotificationType.Title);
            Assert.Equal(existingNotificationType.Description, updatedNotificationType.Description);
        }

        [Fact]
        public async Task DeleteAsync_ExistingNotificationTypeId_Should_DeleteNotificationType()
        {
            
            var notificationTypeId = 4;

            
            var result = await _notificationTypeRepository.DeleteAsync(notificationTypeId);

            
            Assert.True(result);
        }
    }
}
