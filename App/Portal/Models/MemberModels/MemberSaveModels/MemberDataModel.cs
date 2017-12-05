using System;
using System.Collections.Generic;

namespace Portal.Models.MemberModels.MemberSaveModels
{
    public class MemberDataModel
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
    }
}