using Portal.Enums;
using System;
using System.Collections.Generic;

namespace Portal.Models.AccountModels
{
    public class RoleModel
    {
        private List<MenuLinkModel> m_menu = new List<MenuLinkModel>();

        /// <summary>
        /// 持有選單列表
        /// </summary>
        public List<MenuLinkModel> MenuList { get { return this.m_menu; } set { this.m_menu = value; } }

        /// <summary>
        /// 角色類別列舉
        /// </summary>
        public RoleTypeEnum RoleType { get; set; }

        /// <summary>
        /// 主索引
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 角色名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色代碼
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 使用者使用數
        /// </summary>
        public int UsedCount { get; set; }

        /// <summary>
        /// 選單使用數
        /// </summary>
        public int MenuCount { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 刪除識別
        /// </summary>
        public bool Disable { get; set; }

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
    }
}