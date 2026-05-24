namespace Fithub.Platform.Services.Agglestone;

public sealed class AgglestoneSettings
{
    public const string SectionName = "Agglestone:Auth";

    public required string TenantId { get; init; }
}
