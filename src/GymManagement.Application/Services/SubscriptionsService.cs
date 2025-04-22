
namespace GymManagement.Application.Services;

public class SubscriptionsService : ISubscriptionsService
{
    public Guid CreateSubscription(string subscriptionType, Guid adminId)
    {
        return Guid.NewGuid();
    }
}