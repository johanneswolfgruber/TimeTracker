namespace TimeTracker.Domain;

public class ExportService : IExportService
{
    private readonly IMediator _mediator;

    public ExportService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<ExportTrackingsResponse> Handle(ExportTrackingsRequest request,
        CancellationToken cancellationToken)
    {
        var trackings = await GetTrackings(request.Trackings, cancellationToken)
            .ToListAsync(cancellationToken: cancellationToken);

        var orderedTrackings = trackings.OrderBy(x => x.StartTime).ToList();

        CreateExcelFile(orderedTrackings, request.FilePathWithoutExtension);

        return new ExportTrackingsResponse();
    }

    private async IAsyncEnumerable<TrackingDto> GetTrackings(IEnumerable<Guid> trackingIds,
        CancellationToken cancellationToken)
    {
        foreach (var trackingId in trackingIds)
        {
            var response = await _mediator.Send(new GetTrackingRequest(trackingId), cancellationToken);
            if (response.Tracking is null)
            {
                continue;
            }

            yield return response.Tracking;
        }
    }

    private static void CreateExcelFile(IReadOnlyList<TrackingDto> trackings, string filePathWithoutExtension)
    {
        using var workbook = new XLWorkbook(".\\Resources\\Leistungsnachweis.xlsx");
        var worksheet = workbook.Worksheet("Tabelle");

        worksheet.Cell("B4").Value =
            $"{trackings[0].StartTime.ToShortDateString()}-{trackings[^1].EndTime?.ToShortDateString()}";

        var index = 7;
        var currentDate = trackings[0].StartTime.Date;
        foreach (var tracking in trackings)
        {
            var date = tracking.StartTime.Date;

            if (date > currentDate)
            {
                currentDate = date;
                index++;
            }

            worksheet.Cell($"A{index}").Value = date.ToShortDateString();
            worksheet.Cell($"A{index}").SetDataType(XLDataType.DateTime);
            worksheet.Cell($"A{index}").Style.DateFormat.Format = "dd.MM.yyyy";

            worksheet.Cell($"B{index}").Value = string.Join(",", tracking.Notes);
            worksheet.Cell($"C{index}").Value = "P3950";

            worksheet.Cell($"D{index}").Value = tracking.StartTime.ToLocalTime().ToShortTimeString();
            worksheet.Cell($"D{index}").SetDataType(XLDataType.TimeSpan);
            worksheet.Cell($"D{index}").Style.DateFormat.Format = "hh:mm";

            worksheet.Cell($"E{index}").Value = tracking.EndTime?.ToLocalTime().ToShortTimeString();
            worksheet.Cell($"E{index}").SetDataType(XLDataType.TimeSpan);
            worksheet.Cell($"E{index}").Style.DateFormat.Format = "hh:mm";

            index++;
        }


        workbook.SaveAs($"{filePathWithoutExtension}" +
                        $"_{ToFileNameFormatString(trackings[0].StartTime.Date)}" +
                        $"-{ToFileNameFormatString(trackings[^1].EndTime?.Date)}.xlsx");
    }
    
    private static string ToFileNameFormatString(DateTime? date)
    {
        if (date is null)
        {
            return string.Empty;
        }
        
        return $"{date.Value.Day:d2}{date.Value.Month:d2}{date.Value.Year:d4}";
    }
}