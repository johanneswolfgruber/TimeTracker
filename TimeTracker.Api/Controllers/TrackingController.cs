namespace TimeTracker.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TrackingController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<TrackingDto>>))]
        [ProducesResponseType(500, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<IEnumerable<TrackingDto>>> GetAllTrackingsAsync()
        {
            var request = new GetAllTrackingsRequest();
            var getTrackingsResponse = await _mediator.Send(request);

            return getTrackingsResponse.Success
                ? Ok(
                    new ApiResponse<IEnumerable<TrackingDto>>(getTrackingsResponse.Value.Trackings)
                )
                : StatusCode(
                    500,
                    new ApiErrorResponse("Failed to get trackings", [getTrackingsResponse.Error])
                );
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TrackingDto>))]
        [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<TrackingDto>> GetAsync(Guid id)
        {
            var request = new GetTrackingRequest(id);
            var getTrackingResponse = await _mediator.Send(request);

            return getTrackingResponse.Success
                ? Ok(new ApiResponse<TrackingDto>(getTrackingResponse.Value.Tracking))
                : NotFound(
                    new ApiErrorResponse("Failed to get tracking", [getTrackingResponse.Error])
                );
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TrackingDto>))]
        [ProducesResponseType(400, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<TrackingDto>> PostAsync(
            [FromBody] StartTrackingRequest startTrackingRequest
        )
        {
            var request = new CreateTrackingRequest(startTrackingRequest.ActivityId);
            var startTrackingResponse = await _mediator.Send(request);

            return startTrackingResponse.Success
                ? Ok(new ApiResponse<TrackingDto>(startTrackingResponse.Value.Tracking))
                : BadRequest(
                    new ApiErrorResponse("Failed to start tracking", [startTrackingResponse.Error])
                );
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse<TrackingDto>))]
        [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
        public async Task<ActionResult<TrackingDto>> PutAsync(
            Guid id,
            [FromBody] TrackingDto tracking
        )
        {
            var request = new UpdateTrackingRequest(tracking);
            var updateTrackingResponse = await _mediator.Send(request);

            return updateTrackingResponse.Success
                ? Ok(new ApiResponse<TrackingDto>(updateTrackingResponse.Value.Tracking))
                : NotFound(
                    new ApiErrorResponse("Failed to stop tracking", [updateTrackingResponse.Error])
                );
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200, Type = typeof(ApiResponse))]
        [ProducesResponseType(404, Type = typeof(ApiErrorResponse))]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var request = new DeleteTrackingRequest(id);
            var deleteActivityResponse = await _mediator.Send(request);

            return deleteActivityResponse.Success
                ? Ok(new ApiResponse("Tracking deleted"))
                : NotFound(
                    new ApiErrorResponse(
                        "Failed to delete tracking",
                        [deleteActivityResponse.Error]
                    )
                );
        }
    }
}
