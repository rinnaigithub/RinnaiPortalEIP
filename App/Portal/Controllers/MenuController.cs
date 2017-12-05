using Portal.Models.MenuModels.MenuListModels;
using Portal.Models.MenuModels.MenuSaveModels;
using Portal.Repositories;
using System;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class MenuController : BaseController
    {
        private MenuRepository m_menuRepository = new MenuRepository();
        private MenuRepository Repository { get { return this.m_menuRepository; } set { this.m_menuRepository = value; } }

        [HttpGet]
        public ActionResult MenuList(int? page, string qry, string target)
        {
            MenuListViewModel model = new MenuListViewModel();
            model.Filter.FilterTargetStr = target;
            model.Filter.QueryString = qry;
            model.Filter.CurrentPage = page ?? 1;
            model.Result = this.Repository.GetMenuList(model.Filter);
            ViewBag.Targer = target;
            ViewBag.SearchStr = qry;
            return View(model);
        }

        [HttpGet]
        public ActionResult MenuAdd()
        {
            return View(new MenuDataModel() { Enabled = true, MenuSort = 0 });
        }

        [HttpPost]
        public ActionResult MenuAdd(MenuDataModel model)
        {
            Exception error = null;
            MenuDataModel result = new MenuDataModel();
            try
            {
                result = this.Repository.MenuSave(model, Enums.DataSaveMode.Add);
                if (result == null)
                    throw new Exception("新增目錄失敗");
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
            {
                ViewBag.AddFail = error.Message;
                return View(model);
            }
            else
                ViewBag.AddFail = "success";

            return RedirectToAction("MenuEdit", new { muID = result.MenuID });
        }

        [HttpGet]
        public ActionResult MenuEdit(string muID)
        {
            if (string.IsNullOrEmpty(muID))
                throw new Exception("查無該目錄");
            MenuDataModel result = this.Repository.GetMenuByID(muID);
            return View(result);
        }

        [HttpPost]
        public ActionResult MenuEdit(MenuDataModel model)
        {
            MenuDataModel result = new MenuDataModel();
            try
            {
                result = this.Repository.MenuSave(model, Enums.DataSaveMode.Edit);
                if (result == null)
                    throw new Exception("更新目錄失敗");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            TempData["ResultMsg"] = "success";
            return RedirectToAction("MenuEdit", new { muID = result.MenuID });
        }

        /// <summary>
        /// 新增一筆根目錄
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubMenuAdd(SubMenuDataModel model)
        {
            Exception error = null;
            try
            {
                bool result = this.Repository.SubMenuSave(model, Enums.DataSaveMode.Add);
            }
            catch (Exception ex)
            {
                error = ex;
            }

            if (error != null)
                TempData["ResultMsg"] = error.Message;
            else
                TempData["ResultMsg"] = "success";

            return RedirectToAction("MenuEdit", new { muID = model.SubMenuID });
        }




        /// <summary>
        /// 刪除目錄
        /// </summary>
        /// <param name="muID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult MenuDelete(string muID)
        {
            bool success = true;
            string msg = string.Empty;
            try
            {
                success = this.Repository.DeleteMenuByID(muID);
                msg = "success";
            }
            catch (Exception ex)
            {
                msg = ex.Message; ;
                success = false;
            }
            return Json(new { success = success, msg = msg });
        }
    }
}