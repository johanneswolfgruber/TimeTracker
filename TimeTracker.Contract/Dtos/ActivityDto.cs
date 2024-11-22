namespace TimeTracker.Contract.Dtos;

public record ActivityDto(Guid Id, Guid UserId, string Name, ICollection<TrackingDto> Trackings);
