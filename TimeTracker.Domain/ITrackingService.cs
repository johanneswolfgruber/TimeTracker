namespace TimeTracker.Domain;

public interface ITrackingService :
    IRequestHandler<StartTrackingRequest, StartTrackingResponse>,
    IRequestHandler<StopTrackingRequest, StopTrackingResponse>,
    IRequestHandler<DeleteTrackingRequest, DeleteTrackingResponse>,
    IRequestHandler<GetTrackingRequest, GetTrackingResponse>,
    IRequestHandler<GetAllTrackingsForActivityRequest, GetAllTrackingsForActivityResponse>,
    IRequestHandler<UpdateTrackingStartTimeRequest, UpdateTrackingStartTimeResponse>,
    IRequestHandler<UpdateTrackingEndTimeRequest, UpdateTrackingEndTimeResponse>,
    IRequestHandler<UpdateTrackingNotesRequest, UpdateTrackingNotesResponse>
{
}
