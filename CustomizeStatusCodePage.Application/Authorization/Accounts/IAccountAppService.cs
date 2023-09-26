using System.Threading.Tasks;
using Abp.Application.Services;
using CustomizeStatusCodePage.Authorization.Accounts.Dto;

namespace CustomizeStatusCodePage.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
