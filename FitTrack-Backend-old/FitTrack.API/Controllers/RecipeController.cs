using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitTrack.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecipeController : ControllerBase
{
}
