using PagedList;
using Portal.Enums;
using Portal.Models.MemberModels;
using Portal.Models.MemberModels.MemberListModels;
using Portal.Models.MemberModels.MemberSaveModels;
using Portal.Modules;
using Portal.Provider;
using PortalDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Repositories
{
    public class MemberRepository
    {
        private PORTALDB m_porDB = new PORTALDB();

        private PORTALDB PorDB { get { return this.m_porDB; } set { this.m_porDB = value; } }
        private Dictionary<string, string> m_departmentData = new Dictionary<string, string>();

        private Dictionary<string, string> DepartmentData
        {
            get
            {
                if (m_departmentData.Count == 0)
                    m_departmentData = this.GetDepartmentData();
                return this.m_departmentData;
            }
        }

        public MemberListResultModel GetMemberList(MemberListFilterModel filterModel)
        {
            MemberListResultModel result = new MemberListResultModel();

            try
            {
                var empList = this.PorDB.Employee.Select(s => new MemberListDataModel()
                {
                    EmpID = s.EmployeeID,
                    EmpName = s.EmployeeName,
                    DepCode = s.DepartmentID_FK,
                    Disable = s.Disabled,
                    ADAccount = s.ADAccount,
                    BuildUserADAccount = s.Creator,
                    BuildDate = s.CreateDate,
                    GenderCode = s.SexType,
                    NationalityCode = s.NationalType
                }).ToList();

                empList.ForEach(f =>
                    {
                        f.DepName = this.GetDepartmentNameByID(f.DepCode);
                        f.BuildDateStr = f.BuildDate.To10CharString();
                        f.NationalityName = this.TranslateNationNationCode(f.NationalityCode);
                        f.GenderDesc = this.GetGenderDescriptionByCode(f.GenderCode);
                    }
                   );
                //Filter
                empList = this.ProcessFilterCondition(filterModel.FilterTargetEnum, filterModel.QueryString, empList);

                int currentPage = filterModel.CurrentPage;
                int pageSize = Convert.ToInt32(PublicStaticMethod.GetConfigAppSetting("DefaultPageSize"));
                result.Data = empList.ToPagedList(currentPage, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }
      
        /// <summary>
        /// 篩選條件
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="model"></param>
        private List<MemberListDataModel> ProcessFilterCondition(MemberListFilterTargetEnum filter, string qry, List<MemberListDataModel> model)
        {
            List<MemberListDataModel> result = new List<MemberListDataModel>();
            switch (filter)
            {
                case MemberListFilterTargetEnum.NotSet:
                    result = model;
                    break;

                case MemberListFilterTargetEnum.EmpId:
                    if (string.IsNullOrEmpty(qry))
                        result = model;
                    else
                        result = model.Where(o => o.EmpID == qry).ToList();
                    break;

                case MemberListFilterTargetEnum.Diasble:
                    bool diasble;
                    switch (qry)
                    {
                        case "true":
                            diasble = true;
                            break;

                        case "false":
                            diasble = false;
                            break;

                        default:
                            diasble = true;
                            break;
                    }
                    result = model.Where(o => o.Disable == diasble).ToList();
                    break;

                case MemberListFilterTargetEnum.Nationality:
                    result = model.Where(o => o.NationalityCode == qry).ToList();
                    break;

                case MemberListFilterTargetEnum.Dep:
                    result = model.Where(o => o.DepCode == qry).ToList();
                    break;

                default:
                    break;
            }
            return result;
        }

        /// <summary>
        /// 取國別名稱
        /// </summary>
        /// <param name="nCode"></param>
        /// <returns></returns>
        public string TranslateNationNationCode(string nCode)
        {
            string nationName = string.Empty;
            switch (nCode)
            {
                case "TAIWAN":
                    nationName = "台灣";
                    break;

                case "JAPAN":
                    nationName = "日本";
                    break;

                case "VIETNAM":
                    nationName = "越南";
                    break;

                case "INDONESIA":
                    nationName = "印尼";
                    break;

                default:
                    break;
            }
            return nationName;
        }

        /// <summary>
        /// 取姓別描述
        /// </summary>
        /// <param name="nCode"></param>
        /// <returns></returns>
        public string GetGenderDescriptionByCode(string gCode)
        {
            string GenderDescription = string.Empty;
            switch (gCode)
            {
                case "M":
                    GenderDescription = "男性";
                    break;

                case "F":
                    GenderDescription = "女性";
                    break;

                default:
                    break;
            }
            return GenderDescription;
        }

        /// <summary>
        /// 儲存一筆會員使用者資料
        /// </summary>
        /// <param name="member"></param>
        public MemberDataModel MemberSave(MemberDataModel member, DataSaveMode mode)
        {
            Employee empProtoModel = new Employee();
            string empID = member.EmpID;
            AccountModule accountModule = new AccountModule();

            try
            {
                if (mode == DataSaveMode.Add)
                {
                    var protoEmp = accountModule.GetSmartManEmployeeProtoDataByID(empID);
                    if (protoEmp == null)
                        throw new Exception("志元資料庫中查無員工編號：" + member.EmpID + " 相關資訊");
                    var portalEmp = accountModule.GetPrototypeEmployeeByID(member.EmpID);
                    if (portalEmp != null)
                        throw new Exception("Portal資料庫中已有員工編號：" + member.EmpID + " 的資料");
                    empProtoModel = new Employee();
                    empProtoModel.CreateDate = DateTime.UtcNow.AddHours(8);
                    empProtoModel.Creator = SignInProvider.Instance.User.ADAccount;
                }
                else
                {
                    empProtoModel = accountModule.GetPrototypeEmployeeByAccountID(member.ADAccount, this.PorDB);
                }
                empProtoModel.EmployeeID = member.EmpID;
                empProtoModel.EmployeeName = member.EmpName;
                empProtoModel.ADAccount = member.ADAccount;
                empProtoModel.CostDepartmentID = member.CostDepID;
                empProtoModel.DepartmentID_FK = member.DepID;
                empProtoModel.Disabled = member.Disable;
                empProtoModel.DisabledDate = member.Disable ? (DateTime?)Convert.ToDateTime(member.DisableDate) : null;
                empProtoModel.NationalType = member.Nationality;
                empProtoModel.SexType = member.Gender;
                empProtoModel.Modifier = SignInProvider.Instance.User.ADAccount;
                empProtoModel.ModifyDate = DateTime.UtcNow.AddHours(8);
                this.ProcessRoleSave(empID, member.Role);
                if (mode == DataSaveMode.Edit)
                    this.PorDB.Entry(empProtoModel).State = EntityState.Modified;
                else
                    this.PorDB.Employee.Add(empProtoModel);
                this.PorDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            MemberDataModel result = this.GetMemberDataByID(empID);
            return result;
        }

        public void ProcessRoleSave(string empID, List<string> newRoleList)
        {
            try
            {
                var targetData = this.PorDB.PTUserRoleMap;
                List<PTUserRoleMap> newRoles = new List<PTUserRoleMap>();
                //刪除不屬於此次新增的選項
                targetData.RemoveRange(targetData
                              .Where(x => x.MAP_USRID == empID && !newRoleList.Contains(x.MAP_ROLEID.ToString())));
                this.PorDB.SaveChanges();
                //取出剩下的role
                var currentRoles = targetData.Where(o => o.MAP_USRID == empID).ToList();
                List<string> currentRoleList = new List<string>();
                currentRoles.ForEach(f =>
                {
                    if (!currentRoleList.Contains(f.MAP_ROLEID.ToString()))
                        currentRoleList.Add(f.MAP_ROLEID.ToString());
                });
                var RemainingList = new List<string>();

                //得出剩餘要新增的role
                foreach (var role in newRoleList)
                {
                    if (!currentRoleList.Contains(role))
                        RemainingList.Add(role);
                }

                foreach (var role in RemainingList)
                {
                    newRoles.Add(new PTUserRoleMap()
                    {
                        MAP_USRID = empID,
                        MAP_ROLEID = Convert.ToInt32(role)
                    });
                }
                targetData.AddRange(newRoles);
                this.PorDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取得一筆使用者資料
        /// </summary>
        /// <returns></returns>
        public MemberDataModel GetMemberDataByID(string empID)
        {
            MemberDataModel member = new MemberDataModel();
            AccountModule accountModule = new AccountModule();
            var empProtoModel = accountModule.GetPrototypeEmployeeByID(empID);
            if (empProtoModel == null)
                throw new Exception("無法取得帳號訊息");
            member.EmpID = empProtoModel.EmployeeID;
            member.EmpName = empProtoModel.EmployeeName;
            member.ADAccount = empProtoModel.ADAccount;
            member.CostDepID = empProtoModel.CostDepartmentID;
            member.DepID = empProtoModel.DepartmentID_FK;
            member.Disable = empProtoModel.Disabled;
            member.DisableDate = empProtoModel.Disabled == true ? empProtoModel.DisabledDate.To10CharString() : string.Empty;
            member.Nationality = empProtoModel.NationalType;
            member.Gender = empProtoModel.SexType;
            member.UpdateUserADAccount = empProtoModel.Modifier;
            member.UpdateDate = empProtoModel.ModifyDate;
            member.Role = this.GetUserRoleByUserID(empID);
            return member;
        }

        /// <summary>
        /// 取得該使用者所有角色
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public List<string> GetUserRoleByUserID(string empID)
        {
            List<string> roleList = new List<string>();
            var mapDataList = this.PorDB.PTUserRoleMap.Where(o => o.MAP_USRID == empID).ToList();
            foreach (var map in mapDataList)
                roleList.Add(map.MAP_ROLEID.ToString());
            return roleList;
        }

        /// <summary>
        /// 取得部門名稱
        /// </summary>
        /// <param name="depID"></param>
        /// <returns></returns>
        public string GetDepartmentNameByID(string depID)
        {
            string depName = string.Empty;
            var depData = this.DepartmentData;
            depData.TryGetValue(depID, out depName);
            return depName;
        }

        /// <summary>
        /// 取得部門名稱做為catch
        /// </summary>
        /// <param name="depID"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetDepartmentData()
        {
            var dict = this.PorDB.Department.Select(t => new { t.DepartmentID, t.DepartmentName })
                   .ToDictionary(t => t.DepartmentID, t => t.DepartmentName);
            return dict;
        }

        /// <summary>
        ///  取得部門資料轉下拉選單模型
        /// </summary>
        /// <returns></returns>
        public SelectList GetProtoDepartmentDataToSelectList()
        {
            var result = this.PorDB.Department.ToList();
            var list = new SelectList(result, "DepartmentID", "DepartmentName");
            return list;
        }

        /// <summary>
        /// 取得所有角色轉下拉選單模型
        /// </summary>
        /// <returns></returns>
        public SelectList GetProtoRoleDataToSelectList()
        {
            var result = this.PorDB.PTRole.ToList();
            var list = new SelectList(result, "ID", "ROLE_NM");
            return list;
        }
    }
}