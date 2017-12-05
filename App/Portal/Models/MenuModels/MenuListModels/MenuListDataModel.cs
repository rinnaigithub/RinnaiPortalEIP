using System;

namespace Portal.Models.MenuModels.MenuListModels
{
    public class MenuListDataModel
    {
        /// <summary>
        /// 目錄ID
        /// </summary>
        public string MenuID { get; set; }
        /// <summary>
        /// 上層ID
        /// </summary>
        public string ParentMenuID { get; set; }

        /// <summary>
        /// 目錄名稱
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 目錄狀態
        /// </summary>
        public bool MenuStatus { get; set; }

        /// <summary>
        /// 選單呈現狀態
        /// </summary>
        public bool MenuDisplayStatus { get; set; }

        /// <summary>
        /// 建立者
        /// </summary>
        public string BuildUserADAccount { get; set; }

        /// <summary>
        ///建立日
        /// </summary>
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// 日期10碼字串
        /// </summary>
        public string BuildDateStr { get; set; }

        /// <summary>
        /// 是否已經製作
        /// </summary>
        public bool IsMade { get; set; }


        /// <summary>
        /// 子目錄數量
        /// </summary>
        public int SubMenuCount { get; set; }
    }
}