namespace Portal.Models.MemberModels.MemberListModels
{
    public class MemberListViewModel
    {
        private MemberListResultModel m_result = new MemberListResultModel();
        public MemberListResultModel Result { get { return this.m_result; } set { this.m_result = value; } }
        private MemberListFilterModel m_filter = new MemberListFilterModel();
        public MemberListFilterModel Filter { get { return this.m_filter; } set { this.m_filter = value; } }
    }
}