using Fithub.Platform.Domain.Agglestone;

namespace Fithub.Platform.Services.Agglestone;

public interface IAgglestoneUserService
{
    Task<AgglestoneUserDto?> GetUserAsync(string userId, CancellationToken ct = default);
}
