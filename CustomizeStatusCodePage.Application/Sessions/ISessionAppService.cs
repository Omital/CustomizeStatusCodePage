using System.Threading.Tasks;
using Abp.Application.Services;
using CustomizeStatusCodePage.Sessions.Dto;

namespace CustomizeStatusCodePage.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
