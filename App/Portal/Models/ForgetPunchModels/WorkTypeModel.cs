using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.Models.ForgetPunchModels
{
    /*
    WORKTIME (Mapping DUTYWORK.WORKTYPE)
    公司班別時間記錄檔
    MorningTime  Character4 上班時間
    OffWorkTime Character 4 下班時間
    AddWorkTime  Character4 開始加班時間
    OverWorkTime Character4 逾時加班時間
    select MorningTime,OffWorkTime,AddWorkTime,OverWorkTime ,* from WORKTIME where WORKTYPE = 'A'
    */
    public class WorkTypeModel
    {
        /// <summary>
        /// 班別代碼
        /// </summary>
        public string TypeCode { get; set; }

        /// <summary>
        /// 預設上班時間
        /// </summary>
        public string DefaultWorkBeginTIme { get; set; }

        /// <summary>
        /// 預設下班時間
        /// </summary>
        public string DefaultWorkEndTIme { get; set; }

        /// <summary>
        /// 預設加班上班時間
        /// </summary>
        public string DefaultWorkOverTimeBeginTIme { get; set; }

        /// <summary>
        /// 預設加班下班時間
        /// </summary>
        public string DefaultWorkOverTimeEndTIme { get; set; }
    }
}