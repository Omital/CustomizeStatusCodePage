using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CustomizeStatusCodePage.MultiTenancy;

namespace CustomizeStatusCodePage.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}