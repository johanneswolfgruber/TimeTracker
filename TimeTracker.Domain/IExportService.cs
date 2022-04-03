namespace TimeTracker.Domain;

public interface IExportService :
    IRequestHandler<ExportTrackingsRequest, ExportTrackingsResponse>
{
}