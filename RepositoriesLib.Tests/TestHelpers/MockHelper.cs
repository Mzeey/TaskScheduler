using Antlr.Runtime;
using FluentNHibernate.Conventions.Inspections;
using Moq;
using Mzeey.Entities;
using Mzeey.Repositories;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers;
public abstract class MockHelper<TRepository> : IMockHelper<TRepository> where TRepository : class
{
    protected Mock<TRepository> _repositoryMock;
    public abstract Mock<TRepository> ConfigureRepositoryMock();
    protected abstract List<T> GenerateData<T>(int count) where T : new();

    protected string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString();
    }

    public MockHelper()
    {
        _repositoryMock = new Mock<TRepository>();
    }
}

