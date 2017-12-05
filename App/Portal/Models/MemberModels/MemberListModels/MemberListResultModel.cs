using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.MemberModels.MemberListModels
{
    public class MemberListResultModel
    {
        private IPagedList<MemberListDataModel> m_data;
        public IPagedList<MemberListDataModel> Data { get { return this.m_data; } set { this.m_data = value; } }
    }
}