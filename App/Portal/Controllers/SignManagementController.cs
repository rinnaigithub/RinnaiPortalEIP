using Portal.Models.ForgetPunchModels;
using Portal.Repositories;
using System;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SignManagementController : BaseController
    {
        private MemberRepository m_memberRepository = new MemberRepository();
        private MemberRepository Repository { get { return this.m_memberRepository; } set { this.m_memberRepository = value; } }

        //
        private ForgetPunchRepository m_forgetPunchRepository = new ForgetPunchRepository();

        private ForgetPunchRepository ForgetPunchRepository { get { return this.m_forgetPunchRepository; } set { this.m_forgetPunchRepository = value; } }
        //

        // GET: /SignManagement/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateForgetPunchForm()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateForgetPunchForm(FormCollection form)
        {
            return View();
        }

        /// <summary>
        /// 取得員工行事曆
        /// </summary>
        /// <param name="empID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        public JsonResult GetEmpWorkTimeByID(string empID, string date)
        {
            bool isSuccess = true;
            string msg = string.Empty;
            Portal.Repositories.ForgetPunchRepository.ForgetPunchViewModel result = null;
            try
            {
                result = ForgetPunchRepository.GetForgetPunchViewDataByEmpID(empID, date);
            }
            catch (Exception ex)
            {
                isSuccess = false;
                msg = ex.Message;
            }
            return Json(new { success = isSuccess, msg = msg, data = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CreateOverTimeForm()
        {
            ViewBag.defaultDepList = this.Repository.GetProtoDepartmentDataToSelectList();
            ViewBag.defaultSupportDepList = this.Repository.GetProtoDepartmentDataToSelectList();
            return View();
        }

        [HttpPost]
        public ActionResult CreateOverTimeForm(FormCollection form)
        {
            return View();
        }
    }
}