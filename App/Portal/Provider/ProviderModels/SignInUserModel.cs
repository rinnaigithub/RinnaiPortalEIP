using Portal.Models.AccountModels;
using System.Collections.Generic;

namespace Portal.Provider.ProviderModels
{
    public class SignInUserModel
    {
        /// <summary>
        /// 員工編號
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// AD帳號
        /// </summary>
        public string ADAccount { get; set; }
        /// <summary>
        /// 員工名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 國別
        /// </summary>
        public string NationalType { get; set; }


        public DepartmentModel Department { get; set; }
        public List<RoleModel> Roles { get; set; }

        public List<MenuLinkModel> MenuList { get; set; }
    }
}