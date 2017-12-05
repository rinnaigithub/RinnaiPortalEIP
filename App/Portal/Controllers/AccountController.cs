using MemberAuth.Moduls;
using Portal.Models;
using Portal.Models.AccountModels;
using Portal.Provider;
using Portal.Repositories;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class AccountController : Controller
    {
        private AccountRepository m_accountRepository = new AccountRepository();
        private AccountRepository Repository { get { return this.m_accountRepository; } set { this.m_accountRepository = value; } }

        /// <summary>
        /// 無權限頁面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public ActionResult PermissionDenied()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult SignOnFail()
        {
            return View();
        }
        private static readonly char[] InvalidFilenameChars = Path.GetInvalidFileNameChars();

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Logon()
        {
            if (SignInProvider.Instance.User != null)
                return RedirectToAction("Index", "Home");
            return View(new LogonViewModel());
        }

        //
        // GET: /Account/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Logon(LogonViewModel logonModel)
        {
            if (logonModel == null)
            {
                ViewBag.LoginFail = "請輸入相關登入資訊.";
                return View(new LogonViewModel());
            }

            #region AD驗證

            string adValidReusltMsg = ValidADAccountByCustom(logonModel);
            if (!string.IsNullOrEmpty(adValidReusltMsg))
            {
                ViewBag.LoginFail = adValidReusltMsg;
                return View(logonModel);
            }

            #endregion AD驗證

            #region portal驗證

            adValidReusltMsg = this.Repository.GetEmployeeDataForValidLogin(logonModel.Account);

            if (!string.IsNullOrEmpty(adValidReusltMsg))
            {
                ViewBag.LoginFail = adValidReusltMsg;
                return View(logonModel);
            }
            else
            {
                //建立使用者模型實體
                SignInProvider.Instance.SignIn(logonModel);
            }

            #endregion portal驗證

            //進入預設頁面
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {

            SignInProvider.Instance.SignOut();
            //FormsAuthentication.SignOut();

            ////清除所有的 session
            //Session.RemoveAll();

            ////建立一個同名的 Cookie 來覆蓋原本的 Cookie
            //HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //cookie1.Expires = DateTime.Now.AddYears(-1);
            //Response.Cookies.Add(cookie1);

            ////建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            //HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            //cookie2.Expires = DateTime.Now.AddYears(-1);
            //Response.Cookies.Add(cookie2);

            return RedirectToAction("Logon", "Account");
        }

        /// <summary>
        /// 進行AD帳號驗證 使用FormsAuthenticationTicket驗證
        /// </summary>
        /// <param name="logonModel"></param>
        /// <returns></returns>
        //private string ValidADAccountByFormsAuthenticationTicket(LogonViewModel logonModel)
        //{
        //    #region AD驗證

        //    var now = DateTime.Now;
        //    string domainName = logonModel.DomainName;
        //    string adValidMsg = string.Empty;
        //    string adPath = "LDAP://" + domainName;
        //    LdapAuthentication adAuth = new LdapAuthentication(adPath);
        //    try
        //    {
        //        if (true == adAuth.IsAuthenticated(domainName, logonModel.Account, logonModel.Password))
        //        {
        //            string groups = adAuth.GetGroups();
        //            //取出該會員的角色
        //            //string roles = string.Join(",", user.SystemRoles.Select(x => x.Name).ToArray());
        //            var ticket = new FormsAuthenticationTicket(
        //                version: 1,
        //                name: logonModel.Name,
        //                issueDate: now,
        //                expiration: now.AddMinutes(30),
        //                isPersistent: logonModel.Remember,
        //                userData: logonModel.Account,
        //                cookiePath: FormsAuthentication.FormsCookiePath);
        //            //表單門票加密
        //            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
        //            //放入cookie
        //            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //            Response.Cookies.Add(cookie);
        //        }
        //        else
        //        {
        //            adValidMsg = "請輸入正確的帳號或密碼!";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        adValidMsg = "請輸入正確的帳號或密碼!";
        //    }

        //    #endregion AD驗證

        //    return adValidMsg;
        //}

        /// <summary>
        /// 進行AD帳號驗證
        /// </summary>
        /// <param name="logonModel"></param>
        /// <returns></returns>
        private string ValidADAccountByCustom(LogonViewModel logonModel)
        {
            #region AD驗證

            var now = DateTime.Now;
            string domainName = logonModel.DomainName;
            string adValidMsg = string.Empty;
            string adPath = "LDAP://" + domainName;
            LdapAuthentication adAuth = new LdapAuthentication(adPath);
            try
            {
                if (!adAuth.IsAuthenticated(domainName, logonModel.Account, logonModel.Password))
                    adValidMsg = "請輸入正確的帳號或密碼!";
            }
            catch (Exception)
            {
                adValidMsg = "請輸入正確的帳號或密碼!";
            }

            #endregion AD驗證

            return adValidMsg;
        }
    }
}