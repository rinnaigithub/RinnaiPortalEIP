using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.MemberModels.MemberListModels
{
    public class MemberListViewFilterCollectionModel
    {
        string m_empID = string.Empty;
        string m_diasble = string.Empty;
        string m_nationality = string.Empty;
        string m_dep = string.Empty;


        public string EmpID { get { return this.m_empID; } set { this.m_empID = value; } }
        public string Diasble { get { return this.m_diasble; } set { this.m_diasble = value; } }
        public string Nationality { get { return this.m_nationality; } set { this.m_nationality = value; } }
        public string Dep { get { return this.m_dep; } set { this.m_dep = value; } }
    }
}