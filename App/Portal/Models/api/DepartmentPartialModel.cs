using System.Collections.Generic;

namespace Portal.Models.api
{
    /// <summary>
    /// 對應前端套件select2下拉選單模型
    /// </summary>
    public class DepartmentPartialModel
    {
        private List<DepartmentPartialDataModel> m_results = new List<DepartmentPartialDataModel>();
        public List<DepartmentPartialDataModel> results { get { return this.m_results; } set { this.m_results = value; } }
    }

    public class DepartmentPartialDataModel
    {
        public string id { get; set; }
        public string text { get; set; }
    }
}