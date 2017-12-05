using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Portal.Enums
{
    public enum MemberListFilterTargetEnum
    {
        /// <summary>
        /// 未選取
        /// </summary>
        [Description("未選取")]
        NotSet,
        /// <summary>
        /// 員工
        /// </summary>
        [Description("員工")]
        EmpId,
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Diasble,
        /// <summary>
        /// 國別
        /// </summary>
        [Description("國別")]
        Nationality,
        /// <summary>
        /// 部門
        /// </summary>
        [Description("部門")]
        Dep,
    }
}