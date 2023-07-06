using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoriesLib.Tests.TestHelpers
{
    public interface IMockHelper<TRepository> where TRepository: class
    {
        Mock<TRepository> ConfigureRepositoryMock();
    }
}
