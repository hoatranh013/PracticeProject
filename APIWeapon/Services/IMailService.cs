using APIWeapon.Models;
using System.Threading.Tasks;
namespace APIWeapon.Services
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
        
    }
}
