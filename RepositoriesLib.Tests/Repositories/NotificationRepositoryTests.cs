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
    public class NotificationRepositoryTests
    {
        private readonly MockHelper mockHelper;
        private readonly INotificationRepository notificationRepository;

        public NotificationRepositoryTests()
        {
            mockHelper = new MockHelper();
            notificationRepository = mockHelper.ConfigureNotificationRepositoryMock().Object;
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Notification_To_Repository()
        {
            // Arrange
            var newNotification = new Notification
            {
                RecipientId = $"user-{12}",
                Content = "This is the new Content",
                NotificationTypeId = 12
            };

            // Act
            var createdNotification = await notificationRepository.CreateAsync(newNotification);

            // Assert
            Assert.NotNull(newNotification);
            Assert.Equal(11, createdNotification.Id);
            Assert.Equal(newNotification.RecipientId, createdNotification.RecipientId);
        }

        [Fact]
        public async Task RetreiveAllAsync_ReturnsAllNotifications()
        {
            var notifications = await notificationRepository.RetrieveAllAsync();

            Assert.NotNull(notifications);
            Assert.Equal(10, notifications.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingNotificationId_ReturnsExisitngNotification()
        {
            int notificationId = 10;
            var existingNotification = await notificationRepository.RetrieveAsync(notificationId);

            Assert.NotNull(existingNotification);
            Assert.Equal(notificationId, existingNotification.Id);
        }

        [Fact]
        public async Task RetrieveAllByRecipientAsync_ExistingRecipientId_ReturnsExisitingNotificationOfRecipient()
        {
            var userId = "user-1";
            var recipientNotifications = await notificationRepository.RetrieveAllByRecipientAsync(userId);
            Assert.NotNull(recipientNotifications);
            Assert.Equal(1, recipientNotifications.Count());
        }
        [Fact]
        public async Task UpdateAsync_ExisitingNotificationId_ReturnsUpdatedNotification()
        {
            var notificationToUpdateId = 2;
            var existingNotification = await notificationRepository.RetrieveAsync(notificationToUpdateId);

            Assert.NotNull(existingNotification);

            existingNotification.Content = "edited Content";
            var updatedNotification = await notificationRepository.UpdateAsync(notificationToUpdateId, existingNotification);
            Assert.NotNull(updatedNotification);
            Assert.Equal(existingNotification.Content, updatedNotification.Content);
            Assert.Equal(existingNotification.Id, updatedNotification.Id);
        }
        [Fact]
        public async Task DeleteAsync_ExisitingNotificationId_DeletesNotification()
        {
            var notificationToDeletedId = 1;
            var exisitingNotificaton = await notificationRepository.RetrieveAsync(notificationToDeletedId);

            Assert.NotNull(exisitingNotificaton);

            var isDeleted = await notificationRepository.DeleteAsync(notificationToDeletedId);
            Assert.True(isDeleted);
        }
    }
}
