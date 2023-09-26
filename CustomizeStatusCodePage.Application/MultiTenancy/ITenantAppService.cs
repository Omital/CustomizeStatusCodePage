using Abp.Application.Services;
using Abp.Application.Services.Dto;
using CustomizeStatusCodePage.MultiTenancy.Dto;

namespace CustomizeStatusCodePage.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}
