using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace HandbookApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendWelcomeEmailAsync(string recipientEmail, string fullName, string temporaryPassword, int changePasswordHours = 1)
        {
            var emailSettings = _config.GetSection("EmailSettings");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                emailSettings["SenderName"] ?? "Digital Handbook Admin",
                emailSettings["SenderEmail"]
            ));
            message.To.Add(new MailboxAddress(fullName ?? "Staff Member", recipientEmail));
            message.Subject = "Welcome to Employee Digital Handbook - Account Credentials";

            string expirationHour = TimeOnly.FromTimeSpan(TimeSpan.FromHours(changePasswordHours)).ToString("dd MMMM yyyy");

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 8px; overflow: hidden;'>
                        <div style='background-color: #0056b3; color: white; padding: 20px; text-align: center;'>
                            <h2 style='margin: 0;'>Welcome to Employee Digital Handbook</h2>
                        </div>
                        <div style='padding: 24px; color: #333333;'>
                            <p>Dear <strong>{fullName ?? "Staff Member"}</strong>,</p>
                            <p>An account has been created for you on the Employee Digital Handbook system.</p>
                            
                            <div style='background-color: #f8f9fa; border-left: 4px solid #0056b3; padding: 16px; margin: 20px 0;'>
                                <p style='margin: 0 0 8px 0;'><strong>Work Email:</strong> {recipientEmail}</p>
                                <p style='margin: 0;'><strong>Temporary Password:</strong> <code style='font-size: 16px; background-color: #e9ecef; padding: 2px 6px; border-radius: 4px;'>{temporaryPassword}</code></p>
                            </div>

                            <p style='color: #d9534f; font-weight: bold;'>
                                ⚠️ For security purposes, you are required to log in to the Android App and change this password within {changePasswordHours} hour (by {expirationHour}).
                            </p>

                            <p>Please log in using the mobile application and update your password immediately upon first launch.</p>
                            <br/>
                            <p>Kind regards,<br/><strong>Digital Handbook Administration Team</strong></p>
                        </div>
                    </div>"
            };

            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(
                emailSettings["SmtpServer"],
                int.Parse(emailSettings["Port"] ?? "587"),
                SecureSocketOptions.StartTls
            );
            await client.AuthenticateAsync(emailSettings["Username"], emailSettings["Password"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}