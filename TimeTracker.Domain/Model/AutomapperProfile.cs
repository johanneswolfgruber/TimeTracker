namespace TimeTracker.Domain.Model;

public class AutomapperProfile : Profile
{
    public AutomapperProfile()
    {
        CreateMap<Activity, ActivityDto>().ReverseMap();
        CreateMap<Tracking, TrackingDto>().ReverseMap();
    }
}
