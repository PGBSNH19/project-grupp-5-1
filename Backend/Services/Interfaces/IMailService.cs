using Backend.Models.Mail;
using System.Threading.Tasks;

namespace Backend.Services.Interfaces
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}