namespace TimeTracker.Domain;

public interface IExportService
    : IRequestHandler<ExportTrackingsRequest, Result<ExportTrackingsResponse>> { }
