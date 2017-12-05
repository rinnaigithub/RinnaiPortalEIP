using Portal.Models.MenuModels.MenuSaveModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.RoleModels.RoleSaveModels
{
    public class RoleSaveDataModel
    {
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
        public bool IsDelete { get; set; }

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

        string[] m_agreeMenu = new string[] { };
        string[] m_refuseMenu = new string[] { };
        /// <summary>
        /// 持有的目錄權限
        /// </summary>
        public string[] AgreeMenuCodeList { get { return this.m_agreeMenu; } set { this.m_agreeMenu = value; } }
        /// <summary>
        /// 未持有的目錄權限
        /// </summary>
        public string[] RefuseMenuCodeList { get { return this.m_refuseMenu; } set { this.m_refuseMenu = value; } }
    }
}