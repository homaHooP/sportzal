namespace GymAppApi.Application.Common
{
    public interface IClientOwnedRequest
    {
        Guid GetOwnerId();
    }
}
