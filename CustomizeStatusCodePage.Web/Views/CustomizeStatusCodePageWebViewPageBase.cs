using Abp.Web.Mvc.Views;

namespace CustomizeStatusCodePage.Web.Views
{
    public abstract class CustomizeStatusCodePageWebViewPageBase : CustomizeStatusCodePageWebViewPageBase<dynamic>
    {

    }

    public abstract class CustomizeStatusCodePageWebViewPageBase<TModel> : AbpWebViewPage<TModel>
    {
        protected CustomizeStatusCodePageWebViewPageBase()
        {
            LocalizationSourceName = CustomizeStatusCodePageConsts.LocalizationSourceName;
        }
    }
}