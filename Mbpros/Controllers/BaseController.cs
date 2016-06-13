using Mbpros.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace Mbpros.Controllers
{
    //[AuthorizationFilter]
    [SessionExpire]
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        public BaseController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }

    }
}
