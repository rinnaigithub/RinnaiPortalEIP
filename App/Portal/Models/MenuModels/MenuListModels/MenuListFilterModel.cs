using Portal.Enums;
namespace Portal.Models.MenuModels.MenuListModels
{
    public class MenuListFilterModel
    {
        /// <summary>
        /// 選取頁面
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 查詢狀態
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// 篩選目標
        /// </summary>
        public string FilterTargetStr { get; set; }

        /// <summary>
        /// 取得列表的模式 根目錄或子目錄
        /// </summary>
        public MenuListFilterType FilterType { get; set; }

    }
}