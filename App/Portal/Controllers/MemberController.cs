using Portal.Models.MemberModels;
using Portal.Models.MemberModels.MemberListModels;
using Portal.Provider;
using Portal.Repositories;
using System;
using System.Web.Mvc;
using Newtonsoft.Json;
using Portal.Models.MemberModels.MemberSaveModels;

namespace Portal.Controllers
{
    public class MemberController : BaseController
    {
        private MemberRepository m_memberRepository = new MemberRepository();
        private MemberRepository Repository { get { return this.m_memberRepository; } set { this.m_memberRepository = value; } }

        [HttpGet]
        public ActionResult MemberList(int? page, string qry, string target)
        {
            MemberListViewFilterCollectionModel filterCollectionModel = null;
            var filterCol = Request.QueryString["filterCol"];
            if (!string.IsNullOrEmpty(filterCol))
                filterCollectionModel = JsonConvert.DeserializeObject<MemberListViewFilterCollectionModel>(filterCol);

            MemberListViewModel model = new MemberListViewModel();
            model.Filter.FilterTargetStr = target;
            model.Filter.QueryString = qry;
            model.Filter.CurrentPage = page ?? 1;
            model.Result = this.Repository.GetMemberList(model.Filter);
            ViewBag.Targer = target;
            ViewBag.SearchStr = qry;
            ViewBag.FilterCollection = filterCollectionModel;
            return View(model);
        }

        [HttpGet]
        public ActionResult MemberAdd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MemberAdd(MemberDataModel model)
        {
            Exception error = null;
            MemberDataModel result = new MemberDataModel();
            try
            {
                result = this.Repository.MemberSave(model, Enums.DataSaveMode.Add);
                if (result == null)
                    throw new Exception("新增使用者失敗");
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                ViewBag.AddFail = error.Message;
                return View();
            }
            return RedirectToAction("MemBerEdit", new { empID = result.EmpID });
        }

        [HttpGet]
        public ActionResult MemberEdit(string empID)
        {
            MemberDataModel model = new MemberDataModel();
            try
            {
                if (string.IsNullOrEmpty(empID))
                    empID = SignInProvider.Instance.User.ID;
                model = this.Repository.GetMemberDataByID(empID);
            }
            catch (Exception ex)
            {
                ViewBag.AddFail = ex.Message;
            }
            finally
            {
                ViewBag.depList = this.Repository.GetProtoDepartmentDataToSelectList();
                ViewBag.roleList = this.Repository.GetProtoRoleDataToSelectList();
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult MemberEdit(MemberDataModel model)
        {
            MemberDataModel result = new MemberDataModel();
            try
            {
                result = this.Repository.MemberSave(model, Enums.DataSaveMode.Edit);
                if (result == null)
                    throw new Exception("更新使用者失敗");
            }
            catch (Exception ex)
            {
                ViewBag.AddFail = ex.Message;
            }
            finally
            {
                ViewBag.depList = this.Repository.GetProtoDepartmentDataToSelectList();
                ViewBag.roleList = this.Repository.GetProtoRoleDataToSelectList();
            }
            ViewBag.AddSuccess = "Success";
            return View(model);
        }

        [HttpPost]
        public ActionResult MemberDelete(string memberID)
        {
            return View();
        }
    }
}