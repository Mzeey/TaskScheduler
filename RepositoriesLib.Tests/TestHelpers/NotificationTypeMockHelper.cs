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
    public class NotificationTypeMockHelper : MockHelper<INotificationTypeRepository>
    {
        public NotificationTypeMockHelper() { 
            _repositoryMock = new Mock<INotificationTypeRepository>();
        }
        public override Mock<INotificationTypeRepository> ConfigureRepositoryMock()
        {
            var notificationTypes = GenerateData<NotificationType>(10);
            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(notificationTypes);

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var notificationType = notificationTypes.FirstOrDefault(t => t.Id == id);
                    return notificationType;
                });
            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<NotificationType>()))
                .ReturnsAsync((NotificationType notificationType) =>
                {
                    notificationType.Id = notificationTypes.Count() + 1;
                    notificationTypes.Add(notificationType);
                    return notificationType;
                });

            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<NotificationType>()))
                .ReturnsAsync((int id, NotificationType notificationType) =>
                {
                    var existingNotification = notificationTypes.FirstOrDefault(nt => nt.Id == id);

                    existingNotification = notificationType;
                    return notificationType;
                });

            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var existingNotification = notificationTypes.FirstOrDefault(nt => nt.Id == id);
                    if (existingNotification is null)
                    {
                        return false;
                    }
                    return notificationTypes.Remove(existingNotification);
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var notificationTypes = new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var notificationType = new NotificationType
                {
                    Id = i,
                    Title = $"Notification Type - {i}",
                    Description = $"Description for Notification Type - {i}"
                };
                notificationTypes.Add((T)(object) notificationType);
            }
            return notificationTypes;
        }
    }
}
