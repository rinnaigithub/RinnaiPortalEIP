using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace Portal.Enums
{
    public enum SignFormType
    {
        /// <summary>
        /// 尚未設定
        /// </summary>
        [Description("未設定")]
        NotSet,
        /// <summary>
        /// 新增
        /// </summary>
        [Description("忘刷")]
        ForgetPunch,
        /// <summary>
        /// 編輯
        /// </summary>
        [Description("加班")]
        OverTime,
    }
}