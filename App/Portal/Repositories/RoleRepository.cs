using Newtonsoft.Json;
using Portal.Models.MenuModels.MenuSaveModels;
using Portal.Models.RoleModels;
using Portal.Models.RoleModels.RoleSaveModels;
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
    public class RoleRepository
    {
        private PORTALDB m_porDB = new PORTALDB();

        private PORTALDB PorDB { get { return this.m_porDB; } set { this.m_porDB = value; } }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        public List<RoleListDataModel> GetRoleList()
        {
            List<RoleListDataModel> result = new List<RoleListDataModel>();
            try
            {
                var roleList = this.PorDB.PTRole.Select(s => new RoleListDataModel()
                {
                    ID = s.ID,
                    Name = s.ROLE_NM,
                    Description = s.ROLE_DESC,
                    Code = s.ROLE_CD,
                    Disable = (bool)s.DEL_FG,
                    BuildDate = s.BUD_DTM,
                    BuildUserADAccount = s.BUD_USRID,
                }).ToList();
                roleList.ForEach(f =>
                {
                    List<string> userIDList = new List<string>();
                    f.BuildDateStr = f.BuildDate.To10CharString();
                    f.MenuCount = this.PorDB.PTRoleMenuMap.Where(o => o.MAP_ROLEID == f.ID).Count();
                    userIDList = this.PorDB.PTUserRoleMap.Where(o => o.MAP_ROLEID == f.ID).Select(s => s.MAP_USRID).ToList();
                    var empList = this.PorDB.Employee
                          .Where(o => userIDList.Contains(o.EmployeeID))
                          .OrderBy(i => i.EmployeeID)
                          .Select(s => new UserUsedModel { ID = s.EmployeeID, Name = s.EmployeeName })
                          .ToList();
                    f.UsedUserInfo = empList;
                });
                result = roleList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }



        /// <summary>
        /// 取得一筆角色檔
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public RoleSaveDataModel GetRoleByID(int roleID)
        {
            RoleSaveDataModel roleData = this.PorDB.PTRole.Where(o => o.ID == roleID)
            .Select(s => new RoleSaveDataModel()
           {
               ID = s.ID,
               Name = s.ROLE_NM,
               Code = s.ROLE_CD,
               Description = s.ROLE_DESC,
               IsDelete = (bool)s.DEL_FG
           })
           .FirstOrDefault();

            if (roleData != null)
            {
                List<string> menuIDs = this.PorDB.PTRoleMenuMap
             .Where(o => o.MAP_ROLEID == roleID)
             .OrderBy(s => s.ID)
             .Select(i => i.MAP_MUID)
             .ToList();
                MenuModule muModule = new MenuModule();
                List<MenuDataModel> menuData = muModule.GetMenuData();
                roleData.AgreeMenuCodeList = menuData.Where(o => menuIDs.Contains(o.MenuID)).OrderBy(e => e.MenuID).Select(s => s.MenuID).ToArray();
                roleData.RefuseMenuCodeList = menuData.Where(o => !menuIDs.Contains(o.MenuID)).OrderBy(e => e.MenuID).Select(s => s.MenuID).ToArray();
            }

            return roleData;
        }

        /// <summary>
        /// 儲存一筆角色檔
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public int SaveRoleData(FormCollection form)
        {
            int ID = 0;
            string roleID = form["roleID"];
            string roleCode = form["roleCode"];
            string roleName = form["roleName"];
            string roleDesc = form["roleDesc"];
            bool disable = form["Disabled"] == "True" ? true : false;
            string[] agreeAry = form["agreeAry"] == null ? new string[] { } : form["agreeAry"].ToString().Split(',');
            string[] refuseAry = form["refuseAry"] == null ? new string[] { } : form["refuseAry"].ToString().Split(',');
            PTRole role = null;

            if (string.IsNullOrEmpty(roleID))
            {
                var chkRole = this.PorDB.PTRole.Where(o => o.ROLE_CD == roleCode).FirstOrDefault();
                if (chkRole != null)
                    throw new Exception(string.Format("已有相同代碼為：{0} 名稱為：{1}，的角色，請重新定義", roleCode, roleName));
                role = new PTRole()
                {
                    BUD_USRID = SignInProvider.Instance.User.ADAccount,
                    BUD_DTM = DateTime.UtcNow.AddHours(8)
                };
            }
            else
            {
                int rID = Convert.ToInt32(roleID);
                role = this.PorDB.PTRole.Where(o => o.ID == rID).First();
            }
            role.ROLE_CD = roleCode;
            role.ROLE_NM = roleName;
            role.ROLE_DESC = roleDesc;
            role.DEL_FG = disable;
            role.UPD_DTM = DateTime.UtcNow.AddHours(8);
            role.UPD_USRID = SignInProvider.Instance.User.ADAccount;
            if (!string.IsNullOrEmpty(roleID))
                this.PorDB.Entry(role).State = EntityState.Modified;
            else
                this.PorDB.PTRole.Add(role);

            this.PorDB.SaveChanges();
            ID = role.ID;

            List<PTRoleMenuMap> roleMenuList = new List<PTRoleMenuMap>();
            foreach (var agreeMuenID in agreeAry)
            {
                if (string.IsNullOrEmpty(agreeMuenID))
                    continue;
                PTRoleMenuMap m = new PTRoleMenuMap()
                {
                    MAP_ROLEID = ID,
                    MAP_MUID = agreeMuenID
                };
                roleMenuList.Add(m);
            }

            this.PorDB.PTRoleMenuMap.RemoveRange(this.PorDB.PTRoleMenuMap.Where(o => o.MAP_ROLEID == ID));
            this.PorDB.PTRoleMenuMap.AddRange(roleMenuList);
            this.PorDB.SaveChanges();

            return ID;
        }

        /// <summary>
        /// 刪除一筆角色
        /// </summary>
        /// <param name="roleID"></param>
        public void DeleteRoleByID(int roleID)
        {
            try
            {
                var chkHasUsed = this.PorDB.PTUserRoleMap.Where(o => o.MAP_ROLEID == roleID).ToList();
                if (chkHasUsed.Count > 0)
                    throw new Exception("尚有使用者持有該角色，故無法刪除該角色.");
                this.PorDB.PTRoleMenuMap.RemoveRange(this.PorDB.PTRoleMenuMap.Where(o => o.MAP_ROLEID == roleID));
                this.PorDB.PTUserRoleMap.RemoveRange(this.PorDB.PTUserRoleMap.Where(o => o.MAP_ROLEID == roleID));
                this.PorDB.PTRole.RemoveRange(this.PorDB.PTRole.Where(o => o.ID == roleID));
                this.PorDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}