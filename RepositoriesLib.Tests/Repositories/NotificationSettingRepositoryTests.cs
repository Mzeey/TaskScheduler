using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using RepositoriesLib.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RepositoriesLib.Tests.Repositories
{
    public class NotificationSettingRepositoryTests
    {
        private readonly IMockHelper<INotificationSettingRepository> _mockHelper;
        private readonly INotificationSettingRepository _notificationSettingRepository;

        public NotificationSettingRepositoryTests()
        {
            _mockHelper = new NotificationSettingRepositoryMockHelper();
            _notificationSettingRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_NotificationSetting_To_Repository()
        {
            // Arrange
            var newNotificationSetting = new NotificationSetting
            {
                UserId = "user-1",
                NotificationTypeId = 1,
                IsEnabled = true
            };

            // Act
            var createdNotificationSetting = await _notificationSettingRepository.CreateAsync(newNotificationSetting);

            // Assert
            Assert.NotNull(createdNotificationSetting);
            Assert.Equal(newNotificationSetting.UserId, createdNotificationSetting.UserId);
            Assert.Equal(newNotificationSetting.NotificationTypeId, createdNotificationSetting.NotificationTypeId);
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllNotificationSettings()
        {
            // Act
            var notificationSettings = await _notificationSettingRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(notificationSettings);
            Assert.Equal(10, notificationSettings.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingNotificationSettingId_ReturnsExistingNotificationSetting()
        {
            // Arrange
            int notificationSettingId = 1;

            // Act
            var existingNotificationSetting = await _notificationSettingRepository.RetrieveAsync(notificationSettingId);

            // Assert
            Assert.NotNull(existingNotificationSetting);
            Assert.Equal(notificationSettingId, existingNotificationSetting.Id);
        }

        [Fact]
        public async Task RetrieveAllByUserId_ExistingUserId_ReturnsExistingNotificationSettingsOfUser()
        {
            // Arrange
            string userId = "user-1";

            // Act
            var userNotificationSettings = await _notificationSettingRepository.RetrieveAllByUserId(userId);

            // Assert
            Assert.NotNull(userNotificationSettings);
            Assert.Equal(1, userNotificationSettings.Count());
        }

        [Fact]
        public async Task RetrieveByUserIdAndNotificationTypeId_ExistingUserIdAndNotificationTypeId_ReturnsExistingNotificationSetting()
        {
            // Arrange
            string userId = "user-1";
            int notificationTypeId = 1;

            // Act
            var existingNotificationSetting = await _notificationSettingRepository.RetrieveByUserIdAndNotificationTypeId(userId, notificationTypeId);

            // Assert
            Assert.NotNull(existingNotificationSetting);
            Assert.Equal(userId, existingNotificationSetting.UserId);
            Assert.Equal(notificationTypeId, existingNotificationSetting.NotificationTypeId);
        }

        [Fact]
        public async Task UpdateAsync_ExistingNotificationSettingId_ReturnsUpdatedNotificationSetting()
        {
            // Arrange
            int notificationSettingId = 1;
            var existingNotificationSetting = await _notificationSettingRepository.RetrieveAsync(notificationSettingId);

            // Act
            existingNotificationSetting.IsEnabled = false;
            var updatedNotificationSetting = await _notificationSettingRepository.UpdateAsync(notificationSettingId, existingNotificationSetting);

            // Assert
            Assert.NotNull(updatedNotificationSetting);
            Assert.False(updatedNotificationSetting.IsEnabled);
            Assert.Equal(notificationSettingId, updatedNotificationSetting.Id);
        }

        [Fact]
        public async Task DeleteAsync_ExistingNotificationSettingId_DeletesNotificationSetting()
        {
            // Arrange
            int notificationSettingId = 1;
            var existingNotificationSetting = await _notificationSettingRepository.RetrieveAsync(notificationSettingId);

            // Act
            var isDeleted = await _notificationSettingRepository.DeleteAsync(notificationSettingId);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
