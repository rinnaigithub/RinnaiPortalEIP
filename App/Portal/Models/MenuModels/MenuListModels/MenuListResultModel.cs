using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.MenuModels.MenuListModels
{
    public class MenuListResultModel
    {
        private IPagedList<MenuListDataModel> m_data;
        public IPagedList<MenuListDataModel> Data { get { return this.m_data; } set { this.m_data = value; } }
    }
}