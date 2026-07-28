using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskFlow.Application.Exceptions;

namespace TaskFlow.Api.Controllers

{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
            protected int UserId
        {
            get
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if(userId == null)
                {
                    throw new UnauthorizedException("User is not authenticated");
                }
                return int.Parse(userId);
            }
        }
    }
}
