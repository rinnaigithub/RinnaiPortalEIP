using Portal.Models;
using Portal.Modules;
using Portal.Provider.ProviderModels;
using PortalDataEntities.Entities;
using SmartManDataEntities.Entities;
using System;
using System.Web;
using System.Linq;
using Portal.Models.AccountModels;
namespace Portal.Provider
{
    public class SignInProvider
    {
        private PORTALDB m_portalDB = new PORTALDB();
        private PORTALDB PortalDB { get { return this.m_portalDB; } set { this.m_portalDB = value; } }
        private HRISDB m_hrisDB = new HRISDB();
        private HRISDB HrisDB { get { return this.m_hrisDB; } set { this.m_hrisDB = value; } }

        private SignInProvider()
        { }

        private static SignInProvider m_signInProvider = null;
        private SignInUserModel m_user = null;

        /// <summary>
        /// 建立實體
        /// </summary>
        public static SignInProvider Instance
        {
            get
            {
                if (m_signInProvider == null)
                    m_signInProvider = new SignInProvider();
                return m_signInProvider;
            }
        }

        private static HttpContext Context
        { get { return HttpContext.Current; } }

        public SignInUserModel User
        {
            get
            {
                if (Context.Session["UserInfo"] == null)
                    return null;
                if (!(Context.Session["UserInfo"] is SignInUserModel))
                    return null;
                return (SignInUserModel)Context.Session["UserInfo"];
            }
        }

        public void SignIn(LogonViewModel user)
        {
            AccountModule module = new AccountModule();
            Employee userInfo = module.GetPrototypeEmployeeByAccountID(user.Account, this.PortalDB);
            if (userInfo == null)
                throw new Exception("請輸入正確帳號或密碼");
            m_user = this.CreateUser(userInfo);
            Context.Session["UserInfo"] = m_user;
        }

        /// <summary>
        /// 使用者登出系統
        /// </summary>
        /// <returns></returns>
        public bool SignOut()
        {
            if (User != null)
            {
                Context.Session.Remove("UserInfo");
            }
            return true;
        }

        /// <summary>
        /// 建立SignInUserModel實體
        /// </summary>
        /// <param name="empInfo"></param>
        /// <returns></returns>
        private SignInUserModel CreateUser(Employee empInfo)
        {
            AccountModule module = new AccountModule();
            string depId = string.Empty;
            SignInUserModel user = new SignInUserModel();
            user.ID = empInfo.EmployeeID;
            user.ADAccount = empInfo.ADAccount;
            user.Name = empInfo.EmployeeName;
            user.NationalType = empInfo.NationalType;
            depId = empInfo.DepartmentID_FK;
            user.Department = module.GetDepartmentInfoByID(depId);
            user.Roles = module.GetUserRoleAndMenuListByEmpID(user.ID);
            user.MenuList = module.GetMenuListByRoleList(user.Roles);
            return user;
        }
    }
}