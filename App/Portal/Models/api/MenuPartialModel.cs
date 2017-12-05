using System.Collections.Generic;

namespace Portal.Models.api
{
    public class MenuPartialModel
    {
        /// <summary>
        /// 對應前端套件select2下拉選單模型
        /// </summary>

        private List<MenuPartialDataModel> m_results = new List<MenuPartialDataModel>();
        public List<MenuPartialDataModel> results { get { return this.m_results; } set { this.m_results = value; } }

        public class MenuPartialDataModel
        {
            public string id { get; set; }
            public string text { get; set; }
        }
    }
}