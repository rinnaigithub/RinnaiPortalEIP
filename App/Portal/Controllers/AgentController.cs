using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class AgentController : BaseController
    {
        //
        // GET: /Agent/
        public ActionResult Index()
        {
            return View();
        }
    }
}