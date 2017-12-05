namespace Portal.Models.MenuModels.MenuListModels
{
    public class MenuListViewModel
    {
        private MenuListFilterModel m_filtrer = new  MenuListFilterModel();
        private MenuListResultModel  m_result = new MenuListResultModel();
        public MenuListFilterModel Filter { get { return this.m_filtrer; } set { this.m_filtrer = value; } }
        public MenuListResultModel Result { get { return this.m_result; } set { this.m_result = value; } }
    }
}