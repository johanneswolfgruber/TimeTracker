namespace TimeTracker.Contract.Dtos;

public class UserManagerDto(
    IEnumerable<string> errors,
    Dictionary<string, string> userInfo,
    DateTime? expiryDate
)
{
    public IEnumerable<string> Errors { get; } = errors;

    public Dictionary<string, string> UserInfo { get; } = userInfo;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? ExpiryDate { get; } = expiryDate;
}
