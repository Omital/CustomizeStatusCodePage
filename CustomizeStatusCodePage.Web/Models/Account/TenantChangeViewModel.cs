using Abp.AutoMapper;
using CustomizeStatusCodePage.Sessions.Dto;

namespace CustomizeStatusCodePage.Web.Models.Account
{
    [AutoMapFrom(typeof(GetCurrentLoginInformationsOutput))]
    public class TenantChangeViewModel
    {
        public TenantLoginInfoDto Tenant { get; set; }
    }
}