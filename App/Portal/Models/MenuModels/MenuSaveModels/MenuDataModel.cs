using Portal.Models.MenuModels.MenuListModels;

namespace Portal.Models.MenuModels.MenuSaveModels
{
    public class MenuDataModel
    {
        /// <summary>
        /// 目錄ID
        /// </summary>
        public string MenuID { get; set; }

        /// <summary>
        /// 目路名稱
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 目錄對應的Controller名稱
        /// </summary>
        public string MenuPathController { get; set; }

        /// <summary>
        /// 目錄對應的Action名稱
        /// </summary>
        public string MenuPathAction { get; set; }

        /// <summary>
        /// 目錄路徑
        /// </summary>
        public string MenuPath { get; set; }

        /// <summary>
        /// 目錄路徑
        /// </summary>
        public string MenuIcon { get; set; }

        /// <summary>
        /// 啟用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 選單顯示啟用
        /// </summary>
        public bool MenuEnabled { get; set; }

        /// <summary>
        /// 目錄排序
        /// </summary>
        public int MenuSort { get; set; }

        /// <summary>
        /// 上層目錄ID
        /// </summary>
        public string ParentMenuID { get; set; }

        private MenuListViewModel m_subMenuData = new MenuListViewModel();

        public MenuListViewModel SubMenuData
        {
            get { return this.m_subMenuData; }
            set { this.m_subMenuData = value; }
        }
    }
}