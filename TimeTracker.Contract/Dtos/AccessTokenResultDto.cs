namespace TimeTracker.Contract.Dtos;

public class AccessTokenResultDto(string token, DateTime expiryDate)
{
    public string Token { get; init; } = token;

    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpiryDate { get; init; } = expiryDate;
}
