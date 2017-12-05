using Portal.Enums;

namespace Portal.Models.MemberModels.MemberListModels
{
    public class MemberListFilterModel
    {
        /// <summary>
        /// 選取頁面
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// 查詢狀態
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 查詢關鍵字
        /// </summary>
        public string QueryString { get; set; }

        /// <summary>
        /// 篩選目標列舉
        /// </summary>
        public MemberListFilterTargetEnum FilterTargetEnum
        {
            get
            {
                MemberListFilterTargetEnum target = MemberListFilterTargetEnum.NotSet;
                if (!string.IsNullOrEmpty(this.FilterTargetStr))
                {
                    switch (this.FilterTargetStr)
                    {
                        case "empId":
                            target = MemberListFilterTargetEnum.EmpId;
                            break;
                        case "diasble":
                            target = MemberListFilterTargetEnum.Diasble;
                            break;
                        case "nationality":
                            target = MemberListFilterTargetEnum.Nationality;
                            break;
                        case "dep":
                            target = MemberListFilterTargetEnum.Dep;
                            break;
                        default:
                            break;
                    }
                }
                return target;
            }
        }

        /// <summary>
        /// 篩選目標
        /// </summary>
        public string FilterTargetStr { get; set; }

        /// <summary>
        /// 查詢開始日期
        /// </summary>
        public string BeginDate { get; set; }

        /// <summary>
        /// 查詢結束時間
        /// </summary>
        public string EndDate { get; set; }
    }
}