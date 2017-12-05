using System;
using System.Collections.Generic;

namespace Portal.Models.ForgetPunchModels
{
    public class ForgetPunchDataModel
    {
        public string EmpID { get; set; }
        public string ADAccount { get; set; }
        public string EmpName { get; set; }
        public string Nationality { get; set; }
        public string Gender { get; set; }
        public List<string> Role { get; set; }
        public string DepID { get; set; }
        public string CostDepID { get; set; }
        public bool Disable { get; set; }
        public string DisableDate { get; set; }

        public string UpdateUserADAccount { get; set; }
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 班表
        /// </summary>
        public WorkTypeModel WorkType { get; set; }
    }


}