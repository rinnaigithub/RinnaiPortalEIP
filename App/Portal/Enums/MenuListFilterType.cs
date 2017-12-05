using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Portal.Enums
{
    public enum MenuListFilterType
    {
        /// <summary>
        /// 尚未設定
        /// </summary>
        [Description("停用")]
        NotSet,

        /// <summary>
        /// 根目錄列表
        /// </summary>
        [Description("根目錄列表")]
        MainMenuList,

        /// <summary>
        /// 子目錄列表
        /// </summary>
        [Description("子目錄列表")]
        SubMenuList,

    }
}