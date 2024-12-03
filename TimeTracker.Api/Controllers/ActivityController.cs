namespace TimeTracker.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ActivityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<ActivityDto>>))]
        [ProducesResponseType(500, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<IEnumerable<ActivityDto>>> GetAllActivitiesAsync(Guid userId)
        {
            var request = new GetAllActivitiesRequest(userId);
            var getActivitiesResponse = await _mediator.Send(request);

            return getActivitiesResponse.Success
                ? Ok(
                    new ApiResponse<IEnumerable<ActivityDto>>(
                        getActivitiesResponse.Value.Activities
                    )
                )
                : StatusCode(
                    500,
                    new ApiErrorResponse("Failed to get activities", [getActivitiesResponse.Error])
                );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ActivityDto>))]
        [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<ActivityDto>> GetAsync(Guid id)
        {
            var request = new GetActivityRequest(id);
            var getActivityResponse = await _mediator.Send(request);

            return getActivityResponse.Success
                ? Ok(new ApiResponse<ActivityDto>(getActivityResponse.Value.Activity))
                : NotFound(
                    new ApiErrorResponse("Failed to get activity", [getActivityResponse.Error])
                );
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ApiResponse<ActivityDto>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<ActivityDto>> PostAsync(
            [FromBody] CreateActivityRequest request
        )
        {
            var createActivityResponse = await _mediator.Send(request);

            return createActivityResponse.Success
                ? Ok(
                    new ApiResponse<ActivityDto>(
                        createActivityResponse.Value.Activity,
                        "Activity created"
                    )
                )
                : BadRequest(
                    new ApiErrorResponse(
                        "Failed to create activity",
                        [createActivityResponse.Error]
                    )
                );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<ActivityDto>))]
        [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<ActivityDto>> PutAsync(
            Guid id,
            [FromBody] ActivityDto activity
        )
        {
            var request = new UpdateActivityNameRequest(id, activity.Name);
            var updateActivityResponse = await _mediator.Send(request);

            return updateActivityResponse.Success
                ? Ok(
                    new ApiResponse<ActivityDto>(
                        updateActivityResponse.Value.Activity,
                        "Activity updated"
                    )
                )
                : NotFound(
                    new ApiErrorResponse(
                        "Failed to update activity",
                        [updateActivityResponse.Error]
                    )
                );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var request = new DeleteActivityRequest(id);
            var deleteActivityResponse = await _mediator.Send(request);

            return deleteActivityResponse.Success
                ? Ok(new ApiResponse("Activity deleted"))
                : NotFound(
                    new ApiErrorResponse(
                        "Failed to delete activity",
                        [deleteActivityResponse.Error]
                    )
                );
        }
    }
}
