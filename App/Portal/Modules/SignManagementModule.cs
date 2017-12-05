using Portal.Enums;
using PortalDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Modules
{
    public class SignManagementModule
    {
        PORTALDB PORDB = new PORTALDB();
        /// <summary>
        /// 取得一筆簽核單號
        /// </summary>
        /// <param name="signType"></param>
        /// <returns></returns>
        public string GetSignIdentifyNumber(SignFormType signType)
        {
            string identifySignID = string.Empty;

            switch (signType)
            {
                case SignFormType.NotSet:
                    break;
                case SignFormType.ForgetPunch:
                    break;
                case SignFormType.OverTime:
                    break;
                default:
                    break;
            }
            return identifySignID;
        }
    }
}