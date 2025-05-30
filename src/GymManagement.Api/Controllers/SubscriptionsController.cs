using System.Threading.Tasks;
using GymManagement.Contracts.Subscriptions;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using GymManagement.Application.Subscriptions.Queries.GetSubscription;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using DomainSubscriptionType = GymManagement.Domain.Subscriptions.SubscriptionType;
namespace GymManagement.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class SubscriptionsController : ControllerBase
{
    private readonly ISender _mediator;
    public SubscriptionsController(ISender mediator)
    {
        _mediator = mediator;
    
    }

    [HttpPost]
    public async Task<IActionResult> CreateSubscription(CreateSubscriptionRequest request)
    {
        if(!DomainSubscriptionType.TryFromName(request.SubscriptionType.ToString(),out var subscriptionType)){
            return Problem(statusCode: StatusCodes.Status400BadRequest,detail:"Invalid subscription type");
        }
        var command = new CreateSubscriptionCommand(
            subscriptionType,
            request.AdminId);

        var createSubscriptionResult = await _mediator.Send(command);

        return createSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id,request.SubscriptionType)),
            error=> Problem()
        );
    }

    [HttpGet("{subscriptionId:guid}")]
    public async Task<IActionResult> GetSubscription(Guid subscriptionId)
    {
        var query = new GetSubscriptionQuery(subscriptionId);
        var getSubscriptionResult = await _mediator.Send(query);
        return getSubscriptionResult.MatchFirst(
            subscription => Ok(new SubscriptionResponse(subscription.Id,
              Enum.Parse<SubscriptionType>(subscription.SubscriptionType.Name)
            )),
            error=> Problem()
        );
    }
}
