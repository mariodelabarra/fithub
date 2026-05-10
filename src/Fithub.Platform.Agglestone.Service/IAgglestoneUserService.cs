namespace Fithub.Platform.Agglestone.Domain
{
    public interface IAgglestoneUserService
    {
        Task<Dictionary<string, object>> GetUserInfoAsync();
    }
}
