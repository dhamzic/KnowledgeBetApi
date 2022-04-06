namespace KnowledgeBet.API.Api.V1.Models
{
    public class BaseResponse
    {
        public ResponseStatus Status { get; set; }
        public string? LogMessage { get; set; }
    }
    public enum ResponseStatus
    {
        Success,
        Error,
        UnhandledException
    }
}
