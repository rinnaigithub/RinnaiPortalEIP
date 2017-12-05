using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class FileUploadController : Controller
    {
        //
        // GET: /FileUpload/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string Index(string act, int? schoolTypeValue, HttpPostedFileBase image, List<HttpPostedFileBase> images, List<HttpPostedFileBase>  file)
        {
            var fileName = Request.Headers["X-File-Name"];
            var fileSize = Request.Headers["X-File-Size"];
            var fileType = Request.Headers["X-File-Type"];

            var filee = Request.Files;
            var jsonStrModel = JsonConvert.SerializeObject("");
            return jsonStrModel;
        }
    }
}