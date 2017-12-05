using Portal.Models.AccountModels;
using Portal.Provider;
using PortalDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Portal.Authorize
{
    public class AuthAttribute : AuthorizeAttribute
    {
        public string ControllerID { get; internal set; }

        public string ActionID { get; internal set; }


        /// <summary>
        /// 實作權限存取規則
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.ControllerID = filterContext.RouteData.Values["controller"].ToString();
            this.ActionID = filterContext.RouteData.Values["action"].ToString();

            #region 防守條件

            //檢查是否有進此頁面的資格，判斷鋼使用者的規則檔是否有該ControllerName以及ActionName
            RuleHasAuthorityComingSite(filterContext);
            //檢查目錄是否已被停用
            RuleCheckMenuIsDisabled(filterContext);
            //防守是否有權限編輯別人的基本資料
            RuleViewEmployeeData(filterContext);

            #endregion 防守條件

            base.OnAuthorization(filterContext);
        }

        /// <summary>
        /// 檢查是否已通過驗證
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (SignInProvider.Instance.User == null)
            {
                return false;
            }
            return true;
            // if (!httpContext.User.Identity.IsAuthenticated)
            // {
            //     return false;
            // }
            // var claim = ((ClaimsIdentity)httpContext.User.Identity).FindFirst(ClaimTypes.UserData);

            // #region 判斷角色是否 this.Roles 吃action的屬性Role
            // FormsIdentity identity = (FormsIdentity)httpContext.User.Identity;
            // //FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;

            // var roles = this.Roles.Split(',');
            // this.IsAuthorize = roles.Count(role =>
            //System.Web.Security.Roles.IsUserInRole(identity.Ticket.UserData, role)) > 0;

            // #endregion 判斷角色是否 this.Roles 吃action的屬性Role

            //return true;
        }

        /// <summary>
        /// AuthorizeCore返回false走此function
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (SignInProvider.Instance.User == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    action = "Logon",
                    controller = "Account"
                }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }

            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
            //    {
            //        action = "SignOnFail",
            //        controller = "Account"
            //    }));
            //}
            //else
            //{
            //    base.HandleUnauthorizedRequest(filterContext);
            //}
        }
        #region 防守條件

        #region function

        /// <summary>
        /// 比對所有子目錄
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        private bool CheckSubMenuHasMapping(List<RoleModel> roles)
        {
            bool hasAuthority = false;
            foreach (var role in roles)
            {
                foreach (var mu in role.MenuList)
                {
                    if (mu.MenuPathController == this.ControllerID && mu.MenuPathAction == this.ActionID)
                        return true;
                    else
                    {
                        hasAuthority = EachSubMenu(mu);
                        if (hasAuthority)
                            return hasAuthority;
                    }
                }
            }
            return hasAuthority;
        }

        /// <summary>
        /// 遞回尋找所有下層子目錄
        /// </summary>
        /// <param name="menu"></param>
        /// <returns></returns>
        private bool EachSubMenu(MenuLinkModel menu)
        {
            bool hasAuthority = false;
            foreach (var sub in menu.SubMenu)
            {
                if (sub.MenuPathController == this.ControllerID && sub.MenuPathAction == this.ActionID)
                {
                    hasAuthority = true;
                    break;
                }
                else
                {
                    bool isSubHasAuthority = EachSubMenu(sub);
                    if (isSubHasAuthority)
                        return isSubHasAuthority;
                }
            }
            return hasAuthority;
        }

        #endregion function

        /// <summary>
        /// 檢查是否有進此頁面的資格，判斷鋼使用者的規則檔是否有該ControllerName以及ActionName
        /// </summary>
        private void RuleHasAuthorityComingSite(AuthorizationContext filterContext)
        {
            //Home為首頁 大家都可進
            if (this.ControllerID == "Home")
                return;
            //SubMenuAdd為子選單(擁有目錄權限視同有子目錄權限)
            if (this.ActionID == "SubMenuAdd")
                this.ActionID = "MenuAdd";
            if (SignInProvider.Instance.User != null)
            {
                bool chkController = SignInProvider.Instance.User.Roles.Any(a => a.MenuList.Any(s => s.MenuPathController.Contains(this.ControllerID)));

                bool chkAction = SignInProvider.Instance.User.Roles.Any(a =>
                    a.MenuList.Where(s => !string.IsNullOrEmpty(s.MenuPathAction) && s.MenuPathAction.Contains(this.ActionID)).Count() > 0);

                chkAction = CheckSubMenuHasMapping(SignInProvider.Instance.User.Roles);

                //系統管理員 才有刪除功能 若執行呼叫相關功能 則判斷是否為系統管理員
                if (this.ActionID.EndsWith("Delete") || this.ActionID.StartsWith("Delete"))
                {
                    if (SignInProvider.Instance.User.Roles.Where(o => o.RoleType == Enums.RoleTypeEnum.Admin).ToList().Count > 0)
                        chkAction = true;
                }

                if (!chkController || !chkAction)
                {
                    //驗證是否是授權的連線，以及不能是 AJAX 呼叫。
                    if (filterContext.HttpContext.Request.IsAjaxRequest())
                    {
                        //ContentResult cr = new ContentResult();
                        //cr.Content = "<p style=\"color:Red;\">您尚未持有權限執行相關功能!!。</p>";
                        //filterContext.Result = cr;
                    }
                    else
                    {
                        Exception exp = new Exception("尚未持有權限進入此頁面");
                        HandleErrorInfo errorHandle = new HandleErrorInfo(exp, this.ControllerID, this.ActionID);
                        string viewName = "~/Views/Shared/Error.cshtml";
                        var viewReuslt = new ViewResult();
                        viewReuslt.ViewName = viewName;
                        viewReuslt.ViewData = new ViewDataDictionary<HandleErrorInfo>(errorHandle);
                        viewReuslt.ViewData.Add("LogGuId", "8888");
                        filterContext.Result = viewReuslt;
                    }
                }
            }
        }

        /// <summary>
        /// 檢查目錄是否已被停用
        /// </summary>
        /// <param name="filterContext"></param>
        private void RuleCheckMenuIsDisabled(AuthorizationContext filterContext)
        {
            PORTALDB PorDB = new PORTALDB();
            //子目錄
            string path = this.ControllerID + "/" + this.ActionID;
            var func = PorDB.PTFunction.Where(o => o.FN_LINK == path).FirstOrDefault();
            if (func != null)
            {
                string muID = func.MAP_MUID;
                bool enabled = (bool)PorDB.PTMenu.Where(o => o.MUID == muID).First().ACT_FG;
                if (!enabled)
                {
                    Exception exp = new Exception("此目錄已被停用");
                    HandleErrorInfo errorHandle = new HandleErrorInfo(exp, this.ControllerID, this.ActionID);
                    string viewName = "~/Views/Shared/Error.cshtml";
                    var viewReuslt = new ViewResult();
                    viewReuslt.ViewName = viewName;
                    viewReuslt.ViewData = new ViewDataDictionary<HandleErrorInfo>(errorHandle);
                    viewReuslt.ViewData.Add("LogGuId", "0000");
                    filterContext.Result = viewReuslt;
                }
            }

            //根目錄
            var rootDirectory = PorDB.PTFunction.Where(o => o.FN_LINK == this.ControllerID + "/").FirstOrDefault();
            if (rootDirectory != null)
            {
                string rMuID = rootDirectory.MAP_MUID;
                bool enabled = (bool)PorDB.PTMenu.Where(o => o.MUID == rMuID).First().ACT_FG;
                if (!enabled)
                {
                    Exception exp = new Exception("此根目錄已被停用");
                    HandleErrorInfo errorHandle = new HandleErrorInfo(exp, this.ControllerID, this.ActionID);
                    string viewName = "~/Views/Shared/Error.cshtml";
                    var viewReuslt = new ViewResult();
                    viewReuslt.ViewName = viewName;
                    viewReuslt.ViewData = new ViewDataDictionary<HandleErrorInfo>(errorHandle);
                    viewReuslt.ViewData.Add("LogGuId", "0000");
                    filterContext.Result = viewReuslt;
                }
            }
        }

        /// <summary>
        /// 防守是否有權限編輯別人的基本資料
        /// </summary>
        /// <param name="filterContext"></param>
        private void RuleViewEmployeeData(AuthorizationContext filterContext)
        {
            var parameters = filterContext.ActionDescriptor.GetParameters();

            if (SignInProvider.Instance.User != null)
            {
                bool hasAuthority = SignInProvider.Instance.User.Roles.Any(a => a.RoleType == Enums.RoleTypeEnum.Admin);
                if (!hasAuthority)
                {
                    if (ActionID == "MemberEdit")
                    {
                        var emp = parameters.Select(s => new
                        {
                            Name = s.ParameterName,
                            Value = filterContext.HttpContext.Request[s.ParameterName]
                        })
                        .Where(w => w.Name == "empID").FirstOrDefault();
                        string currentEmpID = SignInProvider.Instance.User.ID;
                        if (emp != null)
                        {
                            if (!string.IsNullOrEmpty(emp.Value))
                            {
                                if (currentEmpID != emp.Value)
                                {
                                    Exception exp = new Exception("沒有權限檢視他人資料");
                                    HandleErrorInfo errorHandle = new HandleErrorInfo(exp, this.ControllerID, this.ActionID);
                                    string viewName = "~/Views/Shared/Error.cshtml";
                                    var viewReuslt = new ViewResult();
                                    viewReuslt.ViewName = viewName;
                                    viewReuslt.ViewData = new ViewDataDictionary<HandleErrorInfo>(errorHandle);
                                    viewReuslt.ViewData.Add("LogGuId", "9999");
                                    filterContext.Result = viewReuslt;
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion 防守條件
    }
}