using System.Threading.Tasks;
using GymManagement.Contracts.Subscriptions;
using GymManagement.Application.Subscriptions.Commands.CreateSubscription;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ErrorOr;

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
        var command = new CreateSubscriptionCommand(
            request.SubscriptionType.ToString(),
            request.AdminId);

        var createSubscriptionResult = await _mediator.Send(command);
        
        return createSubscriptionResult.MatchFirst(
            guid => Ok(new SubscriptionResponse(guid,request.SubscriptionType)),
            error=> Problem()
        );
    }
}
