using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Portal.Enums
{
    public enum DataSaveMode
    {

        /// <summary>
        /// 尚未設定
        /// </summary>
        [Description("停用")]
        NotSet,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("新增")]
        Add,
        /// <summary>
        /// 編輯
        /// </summary>
        [Description("編輯")]
        Edit,

    }
}