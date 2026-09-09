namespace HandbookApi.Models
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
