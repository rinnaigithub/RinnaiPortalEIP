using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.MenuModels.MenuSaveModels
{
    public class SubMenuDataModel 
    {
        /// <summary>
        /// 目錄ID
        /// </summary>
        public string SubMenuID { get; set; }

        /// <summary>
        /// 目路名稱
        /// </summary>
        public string SubMenuName { get; set; }

        /// <summary>
        /// 目錄對應的Controller名稱
        /// </summary>
        public string SubMenuPathController { get; set; }

        /// <summary>
        /// 目錄對應的Action名稱
        /// </summary>
        public string SubMenuPathAction { get; set; }

        /// <summary>
        /// 目錄路徑
        /// </summary>
        public string SubMenuPath { get; set; }

        /// <summary>
        /// 目錄路徑
        /// </summary>
        public string SubMenuIcon { get; set; }

        /// <summary>
        /// 啟用
        /// </summary>
        public bool SubEnabled { get; set; }

        /// <summary>
        /// 選單顯示啟用
        /// </summary>
        public bool SubMenuEnabled { get; set; }

        /// <summary>
        /// 目錄排序
        /// </summary>
        public int SubMenuSort { get; set; }

        /// <summary>
        /// 上層目錄ID
        /// </summary>
        public string SubParentMenuID { get; set; }
    }
}