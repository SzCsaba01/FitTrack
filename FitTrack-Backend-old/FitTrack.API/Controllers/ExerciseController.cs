using FitTrack.Data.Contract.Helpers.Requests;
using FitTrack.Data.Contract.Helpers.Responses;
using FitTrack.Service.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ExerciseController : ControllerBase
{
    private readonly IExerciseService _exerciseService;

    public ExerciseController(IExerciseService exerciseService)
    {
        _exerciseService = exerciseService;
    }

    [HttpPut("import-exercises-from-zip")]
    [RequestSizeLimit(200_000_000)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> ImportExercisesFromZipAsync([FromForm] ImportExercisesRequest request)
    {
        await _exerciseService.ImportExercisesFromZipAsync(request);

        var message = new SuccessMessageResponse("You have successfully imported the exercises!");

        return Ok(message);
    }
}
