using Mzeey.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mzeey.Repositories
{
    public interface IPasswordResetTokenRepository
    {
        Task<PasswordResetToken> CreateAsync(PasswordResetToken passwordResetToken);
        Task<PasswordResetToken> UpdateAsync(int id, PasswordResetToken passwordResetToken);
        Task<PasswordResetToken> RetrieveAsync(int id);
        Task<PasswordResetToken> RetrieveByResetToken(string token);
        Task<IEnumerable<PasswordResetToken>> RetrieveAllAsync();
        Task<bool> DeleteAsync(int id);
    }
}
