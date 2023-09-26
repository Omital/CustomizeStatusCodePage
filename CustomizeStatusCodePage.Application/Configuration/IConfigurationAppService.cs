using System.Threading.Tasks;
using Abp.Application.Services;
using CustomizeStatusCodePage.Configuration.Dto;

namespace CustomizeStatusCodePage.Configuration
{
    public interface IConfigurationAppService: IApplicationService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}