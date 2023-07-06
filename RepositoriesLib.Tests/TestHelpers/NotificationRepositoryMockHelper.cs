using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class NotificationRepositoryMockHelper : MockHelper<INotificationRepository>
    {
        public override Mock<INotificationRepository> ConfigureRepositoryMock()
        {
            var notifications = GenerateData<Notification>(10);
            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Notification>()))
                .ReturnsAsync((Notification notification) =>
                {
                    int newIndex = notifications.Count + 1;
                    notification.Id = newIndex;
                    notification.SentDate = DateTime.Now;
                    notification.IsRead = false;
                    notifications.Add(notification);
                    return notification;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync()).ReturnsAsync((IEnumerable<Notification>)notifications.ToList());

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int notificationId) =>
                {
                    Notification notification = notifications.FirstOrDefault(t => t.Id == notificationId);
                    return notification;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllByRecipientAsync(It.IsAny<string>()))
                .ReturnsAsync((string recipientId) =>
                {
                    var result = notifications.Where(n => n.RecipientId.ToUpper() == recipientId.ToUpper());
                    return result;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<Notification>()))
                .ReturnsAsync((int id, Notification notification) =>
                {
                    var existingNotification = notifications.FirstOrDefault(n => n.Id == id);
                    if (existingNotification == null)
                    {
                        return null;
                    }

                    existingNotification = notification;
                    return existingNotification;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int tokenId) =>
                {
                    var existingToken = notifications.FirstOrDefault(t => t.Id == tokenId);
                    if (existingToken == null)
                    {
                        return false;
                    }

                    notifications.Remove(existingToken);
                    return true;
                });
            _repositoryMock.Setup(repo => repo.RetrieveAllByRecipientAsync(It.IsAny<string>()))
                .ReturnsAsync((string recipientId) =>
                {
                    var recipientNotifications = notifications.Where(n => n.RecipientId.ToUpper() == recipientId.ToUpper());
                    return recipientNotifications;
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var notifications = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var notification = new Notification
                {
                    Id = i,
                    RecipientId = $"user-{i}",
                    NotificationTypeId = i,
                    Content = $"Notificaton-{i}'s content",
                    SentDate = DateTime.Now,
                    IsRead = (i % 2 == 0)
                };

                notifications.Add((T)(object) notification);
            }

            return notifications;
        }
    }
}
