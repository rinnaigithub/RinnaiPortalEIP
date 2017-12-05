using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Portal.Modules;
using Portal.Provider;

namespace Portal.Repositories
{

    public class AccountRepository
    {
        /// <summary>
        /// 驗證是否在Portal系統裡有該使用者 (登入)
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public string GetEmployeeDataForValidLogin(string adID)
        {
            bool isHasEmp = true;
            string resultMsg = string.Empty;
            try
            {
                AccountModule module = new AccountModule();
                isHasEmp = (module.GetPrototypeEmployeeByAccountID(adID, new PortalDataEntities.Entities.PORTALDB()) != null);
                if (!isHasEmp)
                    resultMsg = "Portal系統查無該使用者.";
            }
            catch (Exception ex)
            {
                resultMsg = ex.Message;
            }
            return resultMsg;
        }

    }
}