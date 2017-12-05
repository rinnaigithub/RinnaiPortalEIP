using Portal.Models.RoleModels;
using Portal.Models.RoleModels.RoleSaveModels;
using Portal.Modules;
using Portal.Repositories;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class RoleController : BaseController
    {
        private RoleRepository m_roleRepository = new RoleRepository();
        private RoleRepository Repository { get { return this.m_roleRepository; } set { this.m_roleRepository = value; } }


        [HttpGet]
        public ActionResult RoleList()
        {
            RoleListViewModel model = new RoleListViewModel();
            model.Data = this.Repository.GetRoleList();

            return View(model);
        }

        [HttpGet]
        public ActionResult RoleAdd()
        {
            MenuModule muModule = new MenuModule();
            var muList = muModule.GetMenuData();
            ViewBag.MenuList = muList;

            return View();
        }

        [HttpPost]
        public ActionResult RoleAdd(FormCollection form)
        {
            int identityId = 0;
            string redirectUrl = string.Empty;
            try
            {
                identityId = this.Repository.SaveRoleData(form);
            }
            catch (Exception ex)
            {
                redirectUrl = new UrlHelper(Request.RequestContext).Action("RoleAdd", "Role", new { error = true });
                TempData["Error"] = ex.Message;
                return Json(new { Url = redirectUrl });
            }
            TempData["Success"] = "儲存成功";
            redirectUrl = new UrlHelper(Request.RequestContext).Action("RoleEdit", "Role", new { roleID = identityId });
            return Json(new { Url = redirectUrl });
        }

        [HttpGet]
        public ActionResult RoleEdit(int? roleID)
        {
            if (roleID == null)
                throw new Exception("無效的角色ID");
            RoleSaveDataModel model = this.Repository.GetRoleByID((int)roleID);
            if (model == null)
                throw new Exception("找不到ID為：" + roleID + "的角色.");

            MenuModule muModule = new MenuModule();
            var muList = muModule.GetMenuData();
            ViewBag.MenuList = muList;

            return View(model);
        }

        [HttpPost]
        public ActionResult RoleEdit(FormCollection form)
        {
            int identityId = 0;
            string redirectUrl = string.Empty;
            try
            {
                identityId = this.Repository.SaveRoleData(form);
            }
            catch (Exception ex)
            {
                redirectUrl = new UrlHelper(Request.RequestContext).Action("RoleEdit", "Role", new { roleID = form["roleID"] });
                TempData["Error"] = ex.Message;
                return Json(new { Url = redirectUrl });
            }
            TempData["Success"] = "儲存成功";
            redirectUrl = new UrlHelper(Request.RequestContext).Action("RoleEdit", "Role", new { roleID = identityId });
            return Json(new { Url = redirectUrl });
        }

        [HttpPost]
        public JsonResult RoleDelete(int? ID)
        {
            bool success = true;
            string messages = string.Empty;
            try
            {
                this.Repository.DeleteRoleByID((int)ID);
                messages = "刪除成功";
            }
            catch (Exception ex)
            {
                success = false;
                messages = ex.Message;
            }
            var resultJson = Json(new { success = success, messages = messages });
            return resultJson;
        }


    }
}