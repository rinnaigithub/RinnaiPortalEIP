using System;
namespace Portal.Models.MemberModels.MemberListModels
{
    public class MemberListDataModel
    {
        /*
            <th>管理</th>
            <th>員工編號</th>
            <th>員工姓名</th>
            <th>部門代碼</th>
            <th>部門名稱</th>
            <th>AD帳號</th>
            <th>建立者</th>
            <th>建立日</th>
            */

        /// <summary>
        /// 員工編號
        /// </summary>
        public string EmpID { get; set; }

        /// <summary>
        /// 員工姓名
        /// </summary>
        public string EmpName { get; set; }

        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DepCode { get; set; }

        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DepName { get; set; }

        /// <summary>
        /// 國別代碼
        /// </summary>
        public string NationalityCode { get; set; }

        /// <summary>
        /// 國別名稱
        /// </summary>
        public string NationalityName { get; set; }

        /// <summary>
        /// 性別
        /// </summary>
        public string GenderCode { get; set; }

        /// <summary>
        /// 性別描述
        /// </summary>
        public string GenderDesc { get; set; }

        /// <summary>
        /// 停用
        /// </summary>
        public bool Disable { get; set; }

        /// <summary>
        /// ADAccount
        /// </summary>
        public string ADAccount { get; set; }

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