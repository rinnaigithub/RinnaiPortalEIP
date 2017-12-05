using Portal.Models.ForgetPunchModels;
using Portal.Models.MemberModels.MemberSaveModels;
using PortalDataEntities.Entities;
using SmartManDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Portal.Repositories
{
    public class ForgetPunchRepository
    {
        private HRISDB SmartManDB = new HRISDB();
        private PORTALDB PorDB = new PORTALDB();

        /// <summary>
        /// 取得個人行事曆判斷欲申請日期是否為假日
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public bool CheckDateIsHolidayByEmpID(string empID, string date)
        {
            date = date.Replace("-", string.Empty);
            bool isHoliday = false;
            var empData = this.SmartManDB
                .DUTYWORK
                .Where(o =>
                    o.WORKDATE.ToString() == date
                    && o.EMPLOYECD == empID
                ).First();

            if (empData.HOLIDAY == "Y" && empData.H_TYPE.ToString() == "0")
                isHoliday = true;
            return isHoliday;
        }

        /// <summary>
        ///  查當天有無刷卡資料收進(志元員工-日出缺勤記錄檔)
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public bool QueryHasDailyOnOff(string date)
        {
            List<SqlParameter> parameterList = new List<SqlParameter>();
            parameterList.Add(new SqlParameter("@DutyDate", date.Replace("-", string.Empty)));
            string strSQL = @"Select * From Dailyonoff Where DutyDate = @DutyDate and Begintime is not Null and Endtime is not Null";
            var query = this.SmartManDB.DAILYONOFF.SqlQuery(strSQL, parameterList.ToArray()).ToList();
            return query.Count > 0;
        }

        /// <summary>
        /// 提供view下拉選單觸發事件 取得忘刷檢視頁所需資料
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public ForgetPunchViewModel GetForgetPunchViewDataByEmpID(string empID, string date)
        {
            if (!string.IsNullOrEmpty(date))
            {
                #region 判斷假日

                bool isHasRecord = QueryHasDailyOnOff(date);
                if (!isHasRecord)
                    throw new Exception(date + " 卡鐘資料尚未轉入資料庫，無法申請忘刷.");

                #endregion 判斷假日

                #region 判斷假日

                bool isHoliday = CheckDateIsHolidayByEmpID(empID, date);
                if (isHoliday)
                    throw new Exception(date + " 為例假日，無法申請忘刷.");

                #endregion 判斷假日
            }

            ForgetPunchViewModel result = new ForgetPunchViewModel();
            WorkTypeModel tempWorkType = GetEmployeWorkTypeByEmpID(empID);
            result.WorkType = new WorkTypeModel()
            {
                DefaultWorkBeginTIme = string.Format("{0}:{1}", tempWorkType.DefaultWorkBeginTIme.Substring(0, 2), tempWorkType.DefaultWorkBeginTIme.Substring(2, 2)),
                DefaultWorkEndTIme = string.Format("{0}:{1}", tempWorkType.DefaultWorkEndTIme.Substring(0, 2), tempWorkType.DefaultWorkEndTIme.Substring(2, 2))
            };
            MemberRepository MemRepository = new MemberRepository();
            MemberDataModel member = MemRepository.GetMemberDataByID(empID);
            var dep = PorDB.Department.Where(o => o.DepartmentID == member.DepID).FirstOrDefault();
            result.DepartmantName = dep.DepartmentName;

            if (!string.IsNullOrEmpty(date))
            {
                date = date.Replace("-", string.Empty);
                decimal filterDate = Convert.ToDecimal(date);
                var data = SmartManDB.DAILYONOFF.Where(o => o.EMPLOYECD == empID && o.DUTYDATE == filterDate).FirstOrDefault();
                if (data != null)
                {
                    var dutyDateStr = data.DUTYDATE.ToString();
                    result.WorkBeginTime = string.IsNullOrEmpty(data.BEGINTIME) ? "查無打卡記錄" : ToDateTimeString(dutyDateStr, data.BEGINTIME);
                    result.WorkEndTime = string.IsNullOrEmpty(data.ENDTIME) ? "查無打卡記錄" : ToDateTimeString(dutyDateStr, data.ENDTIME);
                }
                else
                {
                    result.WorkBeginTime = "查無打卡記錄";
                    result.WorkEndTime = "查無打卡記錄";
                }
            }
            return result;
        }

        /// <summary>
        /// 將無分隔符號日期字串轉換為日期格式字串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="len"></param>
        /// <param name="remainder"></param>
        /// <returns></returns>
        private string ToDateTimeString(string date, string time)
        {
            List<string> lstStr = new List<string>();
            lstStr.Add(date.Substring(0, 4));
            lstStr.Add(date.Substring(4, 2));
            lstStr.Add(date.Substring(6, 2));

            string result = string.Format("{0} {1}:{2}", string.Join("-", lstStr), time.Substring(0, 2), time.Substring(2, 2));
            return result;
        }

        public class ForgetPunchViewModel
        {
            /// <summary>
            /// 部門名稱
            /// </summary>
            public string DepartmantName { get; set; }

            /// <summary>
            /// 實際上班刷卡時間
            /// </summary>
            public string WorkBeginTime { get; set; }

            /// <summary>
            ///  實際下班刷卡時間
            /// </summary>
            public string WorkEndTime { get; set; }

            /// <summary>
            /// 班別
            /// </summary>
            public WorkTypeModel WorkType { get; set; }
        }

        /// <summary>
        /// 取得員工班表資料
        /// </summary>
        /// <param name="empID"></param>
        public WorkTypeModel GetEmployeWorkTypeByEmpID(string empID)
        {
            var empType = SmartManDB.EMPLOYEE.Where(o => o.EMPLOYECD == empID).FirstOrDefault().WORKTYPE;
            var type = SmartManDB.WORKTIME.Where(o => o.WORKTYPE == empType).FirstOrDefault();
            if (type == null)
                throw new Exception("無法取得班表資訊");
            WorkTypeModel workType = new WorkTypeModel()
            {
                TypeCode = type.WORKTYPE,
                DefaultWorkBeginTIme = type.MORNINGTIME,
                DefaultWorkEndTIme = type.OFFWORKTIME,
                DefaultWorkOverTimeBeginTIme = type.ADDWORKTIME,
                DefaultWorkOverTimeEndTIme = type.OVERWORKTIME,
            };
            return workType;
        }
    }
}