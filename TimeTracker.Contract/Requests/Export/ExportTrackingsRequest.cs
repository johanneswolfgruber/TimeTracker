namespace TimeTracker.Contract.Requests.Export;

public class ExportTrackingsRequest : IRequest<Result<ExportTrackingsResponse>>
{
    public ExportTrackingsRequest(string filePathWithoutExtension, IEnumerable<Guid> trackings)
    {
        FilePathWithoutExtension = filePathWithoutExtension;
        Trackings = trackings;
    }

    public string FilePathWithoutExtension { get; }

    public IEnumerable<Guid> Trackings { get; }
}

public class ExportTrackingsResponse : ApiResponse { }
