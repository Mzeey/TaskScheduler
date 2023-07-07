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
    public class NotificationTemplateRepositoryTests
    {
        private readonly INotificationTemplateRepository _notificationTemplateRepository;
        private readonly NotificationTemplateRepositoryMockHelper _mockHelper;

        public NotificationTemplateRepositoryTests()
        {
            _mockHelper = new NotificationTemplateRepositoryMockHelper();
            _notificationTemplateRepository = _mockHelper.ConfigureRepositoryMock().Object;
        }

        [Fact]
        public async Task RetrieveAllAsync_ReturnsAllTemplates()
        {
            // Arrange

            // Act
            var templates = await _notificationTemplateRepository.RetrieveAllAsync();

            // Assert
            Assert.NotNull(templates);
            Assert.Equal(10, templates.Count());
        }

        [Fact]
        public async Task RetrieveAsync_ExistingTemplateId_ReturnsExistingTemplate()
        {
            // Arrange
            int templateId = 1;

            // Act
            var template = await _notificationTemplateRepository.RetrieveAsync(templateId);

            // Assert
            Assert.NotNull(template);
            Assert.Equal(templateId, template.Id);
        }

        [Fact]
        public async Task RetrieveAllByNotificationTypeAsync_ExistingNotificationTypeId_ReturnsMatchingTemplates()
        {
            // Arrange
            int notificationTypeId = 1;

            // Act
            var templates = await _notificationTemplateRepository.RetrieveAllByNotificationTypeAsync(notificationTypeId);

            // Assert
            Assert.NotNull(templates);
            Assert.All(templates, t => Assert.Equal(notificationTypeId, t.NotificationTypeId));
        }

        [Fact]
        public async Task RetrieveByIdAndNotificationTypeIdAsync_ExistingIds_ReturnsMatchingTemplate()
        {
            // Arrange
            int templateId = 1;
            int notificationTypeId = 1;

            // Act
            var template = await _notificationTemplateRepository.RetrieveByIdAndNotificationTypeIdAsync(templateId, notificationTypeId);

            // Assert
            Assert.NotNull(template);
            Assert.Equal(templateId, template.Id);
            Assert.Equal(notificationTypeId, template.NotificationTypeId);
        }

        [Fact]
        public async Task CreateAsync_Should_AddTemplateToRepository()
        {
            // Arrange
            var newTemplate = new NotificationTemplate
            {
                Body = "Template Body",
                Subject = "Template Subject",
                NotificationTypeId = 1
                
            };

            // Act
            var createdTemplate = await _notificationTemplateRepository.CreateAsync(newTemplate);

            // Assert
            Assert.NotNull(createdTemplate);
            Assert.Equal(11, createdTemplate.Id);
            Assert.Equal(newTemplate.Body, createdTemplate.Body);
            Assert.Equal(newTemplate.Subject, createdTemplate.Subject);
            Assert.Equal(newTemplate.NotificationTypeId, createdTemplate.NotificationTypeId);
        }

        [Fact]
        public async Task UpdateAsync_ExistingTemplateId_ReturnsUpdatedTemplate()
        {
            // Arrange
            int templateId = 2;
            var existingTemplate = await _notificationTemplateRepository.RetrieveAsync(templateId);

            Assert.NotNull(existingTemplate);

            existingTemplate.Body = "Updated Body";
            existingTemplate.Subject = "Updated Subject";

            // Act
            var updatedTemplate = await _notificationTemplateRepository.UpdateAsync(templateId, existingTemplate);

            // Assert
            Assert.NotNull(updatedTemplate);
            Assert.Equal(existingTemplate.Body, updatedTemplate.Body);
            Assert.Equal(existingTemplate.Subject, updatedTemplate.Subject);
            Assert.Equal(existingTemplate.Id, updatedTemplate.Id);
        }

        [Fact]
        public async Task DeleteAsync_ExistingTemplateId_DeletesTemplate()
        {
            // Arrange
            int templateIdToDelete = 1;
            var existingTemplate = await _notificationTemplateRepository.RetrieveAsync(templateIdToDelete);

            Assert.NotNull(existingTemplate);

            // Act
            var isDeleted = await _notificationTemplateRepository.DeleteAsync(templateIdToDelete);

            // Assert
            Assert.True(isDeleted);
        }
    }
}
