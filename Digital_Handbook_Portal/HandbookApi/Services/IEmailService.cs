namespace HandbookApi.Services
{
    public interface IEmailService
    {
        Task SendWelcomeEmailAsync(string recipientEmail, string fullName, string temporaryPassword, int changePasswordHours = 1);
    }
}
