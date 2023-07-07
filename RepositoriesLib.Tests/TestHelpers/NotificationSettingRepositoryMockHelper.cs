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
    public class NotificationSettingRepositoryMockHelper : MockHelper<INotificationSettingRepository>
    {
        public NotificationSettingRepositoryMockHelper() { 
            _repositoryMock = new Mock<INotificationSettingRepository>();
        }
        public override Mock<INotificationSettingRepository> ConfigureRepositoryMock()
        {
            var notificationSettings = GenerateData<NotificationSetting>(10);

            _repositoryMock.Setup(repo => repo.RetrieveAllAsync())
                .ReturnsAsync(() => notificationSettings);
            _repositoryMock.Setup( repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int id)=>
                {
                    return notificationSettings.FirstOrDefault(ns => ns.Id == id);
                });
            _repositoryMock.Setup(repo => repo.RetrieveAllByUserId(It.IsAny<string>()))
                .ReturnsAsync((string userId) =>
                {
                    return notificationSettings.Where(ns => ns.UserId.ToUpper() == userId.ToUpper());
                });
            _repositoryMock.Setup( repo => repo.RetrieveByUserIdAndNotificationTypeId(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync((string userId, int notificationTypeId) =>
                {
                    return notificationSettings.FirstOrDefault(ns => ns.NotificationTypeId == notificationTypeId && ns.UserId.ToUpper() == userId.ToUpper());
                });
            _repositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<NotificationSetting>()))
                .ReturnsAsync((NotificationSetting notificationSetting) =>
                {
                    notificationSetting.Id = notificationSettings.Count + 1;
                    notificationSettings.Add(notificationSetting);
                    return notificationSetting;
                });
            _repositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<NotificationSetting>()))
                .ReturnsAsync((int id, NotificationSetting notificationSetting) =>
                {
                    var exisitingNotificationSetting = notificationSettings.FirstOrDefault(ns => ns.Id == id);
                    if(exisitingNotificationSetting == null)
                    {
                        return null;
                    }
                    exisitingNotificationSetting = notificationSetting;
                    return exisitingNotificationSetting;
                });
            _repositoryMock.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var exisitingNotificationSetting = notificationSettings.FirstOrDefault(ns => ns.Id == id);
                    if (exisitingNotificationSetting == null)
                    {
                        return false;
                    }
                    return notificationSettings.Remove(exisitingNotificationSetting);
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var notificationSettings = new List<T>();
            for(int i = 1; i <= count; i++)
            {
                var notificationSetting = new NotificationSetting
                {
                    Id = i,
                    IsEnabled = (i % 2 == 0),
                    NotificationTypeId = i,
                    UserId = $"user-{i}"
                };
                notificationSettings.Add((T)(object)notificationSetting);
            }
            return notificationSettings;
        }
    }
}
