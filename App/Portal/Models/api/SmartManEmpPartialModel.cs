using System.Collections.Generic;

namespace Portal.Models.api
{
    public class SmartManEmpPartialModel
    {
        private List<EmployeePartialDataModel> m_results = new List<EmployeePartialDataModel>();
        public List<EmployeePartialDataModel> results { get { return this.m_results; } set { this.m_results = value; } }

        public class EmployeePartialDataModel
        {
            /// <summary>
            /// 員工ID
            /// </summary>
            public string id { get; set; }

            /// <summary>
            /// 員工姓名
            /// </summary>
            public string text { get; set; }
        }
    }
}