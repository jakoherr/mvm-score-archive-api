using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.Composers;

namespace Mvm.Score.Archive.Api.Controllers;

[Route("api/")]
[ApiController]
[Produces("application/json")]
public class ApiControllerBase : ControllerBase
{
}
