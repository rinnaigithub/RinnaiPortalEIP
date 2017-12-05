using System.Collections.Generic;

namespace Portal.Models.RoleModels
{
    public class RoleListViewModel
    {
        private List<RoleListDataModel> m_data = new List<RoleListDataModel>();
        public List<RoleListDataModel> Data { get { return this.m_data; } set { this.m_data = value; } }
    }
}