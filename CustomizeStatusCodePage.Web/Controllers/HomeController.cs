using Abp.Web.Mvc.Authorization;
using System.Net;
using System.Web.Mvc;

namespace CustomizeStatusCodePage.Web.Controllers
{
    public class HomeController : CustomizeStatusCodePageControllerBase
    {

        public ActionResult ProduceError500()
        {
            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }

        [AbpMvcAuthorize]
        public ActionResult Index()
        {
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
    }
}