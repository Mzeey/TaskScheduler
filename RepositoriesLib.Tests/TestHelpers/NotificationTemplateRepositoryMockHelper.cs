using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using Remotion.Linq.Clauses.ResultOperators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public class NotificationTemplateRepositoryMockHelper : MockHelper<INotificationTemplateRepository>
    {
        public NotificationTemplateRepositoryMockHelper() { 
            _repositoryMock = new Mock<INotificationTemplateRepository>();
        }
        public override Mock<INotificationTemplateRepository> ConfigureRepositoryMock()
        {
            var templates = GenerateData<NotificationTemplate>(10);

            _repositoryMock.Setup((repo => repo.RetrieveAllAsync()))
                .ReturnsAsync(() =>
                {
                    return templates;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var template = templates.FirstOrDefault(t => t.Id == id);
                    return template;
                });

            _repositoryMock.Setup(repo => repo.RetrieveAllByNotificationTypeAsync(It.IsAny<int>()))
                .ReturnsAsync((int notificationTypeId) =>
                {
                    return templates.Where(t => t.Id == notificationTypeId);
                });

            _repositoryMock.Setup((repo) => repo.RetrieveByIdAndNotificationTypeIdAsync(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((int id, int notificationTypeId) =>
                {
                    return templates.FirstOrDefault(t => t.Id == id && t.NotificationTypeId == notificationTypeId);
                });

            _repositoryMock.Setup((repo) => repo.CreateAsync(It.IsAny<NotificationTemplate>()))
                .ReturnsAsync((NotificationTemplate notificationTemplate) =>
                {
                    notificationTemplate.Id = templates.Count + 1;
                    templates.Add(notificationTemplate);
                    return notificationTemplate;
                });

            _repositoryMock.Setup((repo) => repo.UpdateAsync(It.IsAny<int>(), It.IsAny<NotificationTemplate>()))
                .ReturnsAsync((int id, NotificationTemplate notificationTemplate) =>
                {
                    var existingTemplate = templates.FirstOrDefault(t => t.Id == id);
                    if(existingTemplate == null)
                    {
                        return null;
                    }
                    existingTemplate = notificationTemplate;
                    return existingTemplate;

                });

            _repositoryMock.Setup((repo) => repo.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync((int id) =>
                {
                    var exisitingTemplate = templates.FirstOrDefault(t => t.Id == id);
                    if (exisitingTemplate == null)
                    {
                        return false;
                    }
                    return templates.Remove(exisitingTemplate);
                });

            return _repositoryMock;
        }

        protected override List<T> GenerateData<T>(int count)
        {
            var templates =  new List<T>();
            for (int i = 1; i <= count; i++)
            {
                var template = new NotificationTemplate {
                    Id = i,
                    Body = $"Content for Notification Template - {i}",
                    Subject = $"Subject for Notification Template - {i}",
                    NotificationTypeId = i
                };
                templates.Add((T)(object)template);
            }

            return templates;
        }
    }
}
