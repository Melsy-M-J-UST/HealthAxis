using HealthAxis.Api.Data;

namespace HealthAxis.Api.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User1 GetByEmail(string email);
        User1 Add(User1 user);
        bool UpdateReferenceId(string userId, int referenceId);
        string GenerateNextUserId(string role);
    }
}