using DAL.Models.Common;
using System.Threading.Tasks;

namespace BLL.Services.EmailSetting
{
    public interface IEmailSettings
    {
        // Changed to Task for asynchronous execution
        Task SendEmailAsync(ResetEmail resetEmail);
    }
}