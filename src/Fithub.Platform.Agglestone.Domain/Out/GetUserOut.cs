using System.Security.Claims;

namespace Fithub.Platform.Agglestone.Domain.Out
{
    public record GetUserOut(bool IsAuthenticated, UserDetailsOut? User);

    public record UserDetailsOut(string Name,
        string Email,
        string UserId,
        IEnumerable<ClaimsOut> Claims);

    public record ClaimsOut(string Type, string Value);
}
