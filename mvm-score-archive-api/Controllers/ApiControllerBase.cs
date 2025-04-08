using Microsoft.AspNetCore.Mvc;
using Mvm.Score.Archive.Service.ErrorHandling;

namespace Mvm.Score.Archive.Api.Controllers;

[ApiController]
[Produces("application/json")]
[Route("api/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected IActionResult ReturnProblemDetail(CustomError error) =>
        this.Problem(
                detail: error.Description,
                title: error.Title,
                statusCode: (int)error.HttpStatusCode);
}
