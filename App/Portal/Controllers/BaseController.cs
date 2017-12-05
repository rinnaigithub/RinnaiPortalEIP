using Portal.Attributes;
using Portal.Authorize;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    [Auth(Roles = "Admin,User")]
    [ErrorHandler]
    public class BaseController : Controller
    {

    }
}