using Portal.Enums;
using Portal.Models.AccountModels;
using Portal.Repositories;
using PortalDataEntities.Entities;
using SmartManDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Modules
{
    public class AccountModule
    {
        private PORTALDB m_db = new PORTALDB();

        private PORTALDB DB { get { return this.m_db; } set { this.m_db = value; } }

        private HRISDB m_hriDB = new HRISDB();
        private HRISDB HriDB { get { return this.m_hriDB; } set { this.m_hriDB = value; } }

        /// <summary>
        /// 取得志元員工資料原型模型
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public EMPLOYEE GetSmartManEmployeeProtoDataByID(string empID)
        {
            var emp = this.HriDB.EMPLOYEE.Where(o => o.EMPLOYECD == empID).FirstOrDefault();
            return emp;
        }

        /// <summary>
        /// 取得Portal系統內的員工資料基本檔 依據ID
        /// </summary>
        /// <param name="empID"></param>
        /// <returns></returns>
        public Employee GetPrototypeEmployeeByID(string empID)
        {
            Employee emp = this.DB.Employee.Where(o => o.EmployeeID == empID).FirstOrDefault();
            return emp;
        }

        /// <summary>
        /// 取得Portal系統內的員工資料基本檔 依據ADAccount
        /// </summary>
        /// <param name="adID"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public Employee GetPrototypeEmployeeByAccountID(string adID, PORTALDB db)
        {
            Employee emp = db.Employee.Where(o => o.ADAccount == adID).FirstOrDefault();
            return emp;
        }

        /// <summary>
        /// 取得部門資訊
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        public DepartmentModel GetDepartmentInfoByID(string depId)
        {
            DepartmentModel dep = new DepartmentModel();
            var depInfo = this.DB.Department.Where(o => o.DepartmentID == depId).FirstOrDefault();
            if (depInfo == null)
                throw new Exception("無法取得部門資訊");
            dep.ID = depInfo.DepartmentID;
            dep.Name = depInfo.DepartmentName;
            return dep;
        }

        /// <summary>
        /// 取得該User的全部角色(過濾已停用角色)
        /// </summary>
        /// <param name="empID"></param>
        public List<RoleModel> GetUserRoleAndMenuListByEmpID(string empID)
        {
            var userRoles = this.DB.PTUserRoleMap.Where(o => o.MAP_USRID == empID).ToList();
            List<RoleModel> roleList = new List<RoleModel>();
            foreach (var role in userRoles)
            {
                RoleModel r = this.GetRoleByID((int)role.MAP_ROLEID);
                if (r.Disable == false)
                    roleList.Add(r);
            }
            return roleList;
        }


        /// <summary>
        //  取出所有角色之下的目錄並移除重複
        /// </summary>
        /// <param name="roleList"></param>
        /// <returns></returns>
        public List<MenuLinkModel> GetMenuListByRoleList(List<RoleModel> roleList)
        {
            List<string> muIDList = new List<string>();
            foreach (var r in roleList)
            {
                foreach (var m in r.MenuList)
                {
                    muIDList.Add(m.MenuID);
                    m.SubMenu.ForEach(f =>
                    {
                        muIDList.Add(f.MenuID);
                        muIDList.AddRange(AddSubMenu(f));
                    }
                        );
                }
            }
            muIDList = muIDList.Distinct().ToList();

            List<MenuLinkModel> muModelList = new List<MenuLinkModel>();
            MenuLinkModel model = new MenuLinkModel();
            MenuModule menuModule = new MenuModule();
            foreach (var mu in muIDList)
            {
                var menu = menuModule.GetMenuDataByID(mu);
                model = new MenuLinkModel()
               {
                   MenuID = menu.MenuID,
                   ParentMenuID = menu.ParentMenuID,
                   MenuName = menu.MenuName,
                   Enabled = (bool)menu.Enabled,
                   MenuEnabled = (bool)menu.MenuEnabled,
                   MenuPathController = menu.MenuPathController,
                   MenuPathAction = menu.MenuPathAction,
                   MenuPath = menu.MenuPath,
                   MenuSort = menu.MenuSort,
                   MenuIcon = menu.MenuIcon
               };

                if (mu.Length == 1)
                {
                    if (!muModelList.Contains(model))
                        muModelList.Add(model);
                }
                //找尋第一層
                var find = muModelList.Where(o => o.MenuID == model.ParentMenuID).FirstOrDefault();
                if (find != null)
                    find.SubMenu.Add(model);
                else
                {
                    //找尋無限下層
                    foreach (var m in muModelList)
                    {
                        if (model.ParentMenuID == null)
                            break;
                        var findSubMenu = FindSubMenu(m, model);
                    }
                }
            }
            return muModelList;
        }

        /// <summary>
        /// 尋找無限下層子選單並找尋為model的父選單，若無合就加入該model為子選單
        /// </summary>
        /// <param name="sub"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        private MenuLinkModel FindSubMenu(MenuLinkModel sub, MenuLinkModel model)
        {
            string muID = model.ParentMenuID;
            foreach (var s in sub.SubMenu)
            {
                if (s.MenuID == muID)
                {
                    s.SubMenu.Add(model);
                    return s;
                }
                else
                {
                    var find = FindSubMenu(s, model);
                    if (find != null)
                        return find;
                }
            }
            return null;
        }

        /// <summary>
        /// 取出所有子選單的MenuIID
        /// </summary>
        /// <param name="sub"></param>
        /// <returns></returns>
        private List<string> AddSubMenu(MenuLinkModel sub)
        {
            List<string> muIDlist = new List<string>();
            sub.SubMenu.ForEach(f =>
                {
                    muIDlist.Add(f.MenuID);
                    muIDlist.AddRange(AddSubMenu(f));
                }
                );
            return muIDlist;
        }

        /// <summary>
        /// 取得一個角色模型
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public RoleModel GetRoleByID(int roleID)
        {
            var roleInfo = this.DB.PTRole.Where(o => o.ID == roleID).FirstOrDefault();
            if (roleInfo == null)
                throw new Exception("無法取得角色資訊");
            RoleModel role = new RoleModel();
            role.RoleType = roleInfo.ROLE_CD.ToEnum<RoleTypeEnum>();
            role.ID = roleInfo.ID;
            role.Description = roleInfo.ROLE_DESC;
            role.Code = roleInfo.ROLE_CD;
            role.Name = roleInfo.ROLE_NM;
            role.BuildDate = roleInfo.BUD_DTM;
            role.BuildDateStr = roleInfo.BUD_DTM.To10CharString();
            role.BuildUserADAccount = roleInfo.BUD_USRID;
            role.Disable = (bool)roleInfo.DEL_FG;
            role.MenuList = this.GetRoleMenuByRoleID(roleInfo.ID);
            return role;
        }

        /// <summary>
        /// 取得該角色下所有可瀏覽的目錄功能
        /// </summary>
        /// <param name="roleID"></param>
        private List<MenuLinkModel> GetRoleMenuByRoleID(int roleID)
        {
            List<MenuLinkModel> resultMenuList = new List<MenuLinkModel>();
            var menuList = this.DB.PTRoleMenuMap.Where(o => o.MAP_ROLEID == roleID).OrderBy(s => s.MAP_MUID).ToList();
            foreach (var mu in menuList)
            {
                #region 確認是否已加入過選單 將遍歷底下所有子選單

                bool hasInsert = resultMenuList.Any(o => o.MenuID == mu.MAP_MUID);

                foreach (var sub in resultMenuList.Select(s => s.SubMenu).ToList())
                {
                    foreach (var s in sub)
                    {
                        hasInsert = findSubMenu(s, mu.MAP_MUID);
                        if (hasInsert)
                            break;
                    }
                }

                if (hasInsert)
                    continue;

                resultMenuList.Add(this.GetMenuFunctionAllValidForRoleID(mu.MAP_MUID, roleID));

                #endregion 確認是否已加入過選單 將遍歷底下所有子選單
            }

            return resultMenuList;
        }

        /// <summary>
        /// 利用遞回方式確認無限下層子選單是否已含該目錄
        /// </summary>
        /// <param name="subMenu"></param>
        /// <param name="muID"></param>
        /// <returns></returns>
        private bool findSubMenu(MenuLinkModel subMenu, string muID)
        {
            bool hasInsert = false;
            if (subMenu.MenuID == muID)
                hasInsert = true;
            else
            {
                foreach (var sub in subMenu.SubMenu)
                {
                    hasInsert = findSubMenu(sub, muID);
                    if (hasInsert)
                        return hasInsert;
                }
            }
            return hasInsert;
        }

        /// <summary>
        /// 利用遞迴方式 取得該角色下所有選單功能 無限追朔下層
        /// </summary>
        /// <param name="muID"></param>
        private MenuLinkModel GetMenuFunctionAllValidForRoleID(string muID, int roleID)
        {
            MenuModule menuModule = new MenuModule();
            var menu = menuModule.GetMenuDataByID(muID);
            MenuLinkModel model = new MenuLinkModel()
            {
                MenuID = menu.MenuID,
                ParentMenuID = menu.ParentMenuID,
                MenuName = menu.MenuName,
                Enabled = (bool)menu.Enabled,
                MenuEnabled = (bool)menu.MenuEnabled,
                MenuPathController = menu.MenuPathController,
                MenuPathAction = menu.MenuPathAction,
                MenuPath = menu.MenuPath,
                MenuSort = menu.MenuSort,
                MenuIcon = menu.MenuIcon
            };
            //取下層
            var findSub = this.DB.PTMenu.Join(this.DB.PTRoleMenuMap,
                         t1 => t1.MUID,
                         t2 => t2.MAP_MUID,
                         (m, r) => new
                         {
                             RoleID = r.MAP_ROLEID,
                             ParentMenuID = m.MUPID,
                             MenuID = m.MUID
                         })
                         .Where(s => s.RoleID == roleID && s.ParentMenuID == muID)
                         .ToList();

            foreach (var sub in findSub)
                model.SubMenu.Add(GetMenuFunctionAllValidForRoleID(sub.MenuID, roleID));

            return model;
        }
    }
}