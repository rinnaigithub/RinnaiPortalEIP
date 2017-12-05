using PortalDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Attributes
{
    public class ErrorHandlerAttribute : HandleErrorAttribute
    {
        public string ControllerID { get; internal set; }

        public string ActionID { get; internal set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnException(ExceptionContext filterContext)
        {
            this.ControllerID = filterContext.RouteData.Values["controller"].ToString();
            this.ActionID = filterContext.RouteData.Values["action"].ToString();


            PORTALDB DB = new PORTALDB();
            base.OnException(filterContext);
            Exception exception = filterContext.Exception;
            int logGuId = new System.Random().Next(0, 32767);
            PTSYSLOGERR Log = new PTSYSLOGERR();
            Log.ERR_GID = logGuId;
            Log.ERR_SRC = exception.Source;
            Log.ERR_SMRY = string.Format("messages：{0} 。 innerException：{1}", exception.Message, exception.InnerException);
            Log.ERR_DESC = exception.StackTrace;
            Log.LOG_DTM = DateTime.UtcNow.AddHours(8);
            DB.PTSYSLOGERR.Add(Log);
            DB.SaveChanges();
           

            var typedResult = filterContext.Result as ViewResult;
            if (typedResult != null)
            {
                var tmpModel = typedResult.ViewData.Model;
                typedResult.ViewData = filterContext.Controller.ViewData;
                typedResult.ViewData.Model = tmpModel;
                typedResult.ViewData.Add("LogGuId", logGuId);
                filterContext.Result = typedResult;
            }
        }
    }
}