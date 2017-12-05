using System.ComponentModel;

namespace Portal.Enums
{
    public enum RoleTypeEnum
    {
        /* 資料庫定義
            Admin	管理員
            BBSManage	公佈欄管理
            Manage	主管
            TrainManage	教育訓練管理員
            User	一般使用者
        */

        /// <summary>
        /// 尚未設定
        /// </summary>
        [Description("尚未設定")]
        NotSet,

        /// <summary>
        /// 管理員
        /// </summary>
        [Description("管理員")]
        Admin,

        /// <summary>
        /// 公佈欄管理
        /// </summary>
        [Description("公佈欄管理")]
        BBSManage,

        /// <summary>
        /// 主管
        /// </summary>
        [Description("主管")]
        Manage,

        /// <summary>
        /// 教育訓練管理員
        /// </summary>
        [Description("教育訓練管理員")]
        TrainManage,

        /// <summary>
        /// 一般使用者
        /// </summary>
        [Description("一般使用者")]
        User,

    }
}