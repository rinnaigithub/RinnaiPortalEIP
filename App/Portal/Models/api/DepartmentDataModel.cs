using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.api
{
    public class DepartmentDataModel
    {
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DepartmentID { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 主管員工編號
        /// </summary>
        public string LeaderEmpID { get; set; }

        /// <summary>
        /// 上層部門代號
        /// </summary>
        public string UpperDepartmentID { get; set; }

        /// <summary>
        /// 部門層級 0是最上層
        /// </summary>
        public int DeptLevel { get; set; }

        /// <summary>
        /// 停用
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// 停用日起
        /// </summary>
        public DateTime? DisabledDate { get; set; }

        /// <summary>
        /// 建立者ADAccount
        /// </summary>
        public string BuildADAcount { get; set; }

        /// <summary>
        /// /// 建立日期
        /// </summary>
        public DateTime? BuildDate { get; set; }

        /// <summary>
        /// 更新者ADAccount
        /// </summary>
        public string UpdateADAcount { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }
    }
}