using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using CustomizeStatusCodePage.Configuration.Dto;

namespace CustomizeStatusCodePage.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : CustomizeStatusCodePageAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
