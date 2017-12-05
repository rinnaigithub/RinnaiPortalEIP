using System.Collections.Generic;

namespace Portal.Models.api
{
    /// <summary>
    /// 對應前端套件select2下拉選單模型
    /// </summary>
    public class RolePartialModel
    {
        private List<RolePartialDataModel> m_results = new List<RolePartialDataModel>();
        public List<RolePartialDataModel> results { get { return this.m_results; } set { this.m_results = value; } }

        public class RolePartialDataModel
        {
            public int id { get; set; }
            public string text { get; set; }
        }
    }
}