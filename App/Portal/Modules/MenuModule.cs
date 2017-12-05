using Portal.Enums;
using Portal.Models.MenuModels.MenuSaveModels;
using Portal.Provider;
using PortalDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Portal.Modules
{
    public class MenuModule
    {
        private PORTALDB m_porDB = new PORTALDB();

        private PORTALDB PorDB { get { return this.m_porDB; } set { this.m_porDB = value; } }

        public MenuDataModel SaveMenuData(DataSaveMode mode, MenuDataModel model)
        {
            MenuDataModel result = new MenuDataModel();
            string muID = model.MenuID;
            PTMenu pTMenu = null;
            PTFunction pTFunction = null;

            try
            {
                if (mode == DataSaveMode.Add)
                {
                    var chkHasMenu = this.GetMenuDataByID(muID);
                    if (chkHasMenu != null)
                        throw new Exception("已有MenuID為：" + muID + " 的選單，目錄名稱為：" + chkHasMenu.MenuName);
                    //menu
                    pTMenu = new PTMenu()
                   {
                       BUD_DTM = DateTime.UtcNow.AddHours(8),
                       BUD_USRID = SignInProvider.Instance.User.ADAccount
                   };
                    //function
                    pTFunction = new PTFunction()
                    {
                        BUD_DTM = DateTime.UtcNow.AddHours(8),
                        BUD_USRID = SignInProvider.Instance.User.ADAccount,
                    };
                }
                else if (mode == DataSaveMode.Edit)
                {
                    //menu
                    pTMenu = this.PorDB.PTMenu.Where(o => o.MUID == muID).FirstOrDefault();
                    if (pTMenu == null)
                        throw new Exception("無法取得選單主檔");
                    //function
                    pTFunction = this.GetProtorMentFunctionByID(muID);
                }
                else
                    throw new Exception("[儲存目錄]無法得知的儲存模式");
                pTMenu.MUID = model.MenuID;
                pTMenu.MU_NM = model.MenuName;
                pTMenu.MUICON = model.MenuIcon;
                pTMenu.ACT_FG = model.Enabled;
                pTMenu.MENU_FG = model.MenuEnabled;
                pTMenu.MUPID = model.ParentMenuID;
                pTMenu.UPD_DTM = DateTime.UtcNow.AddHours(8);
                pTMenu.UPD_USRID = SignInProvider.Instance.User.ADAccount;

                pTFunction.MAP_MUID = muID;
                pTFunction.SORT_SEQ = model.MenuSort;
                pTFunction.FN_DIR = model.MenuPathController;
                pTFunction.FN_KEY = model.MenuPathAction;
                pTFunction.FN_LINK = string.Concat(model.MenuPathController, "/", model.MenuPathAction);
                pTFunction.UPD_DTM = DateTime.UtcNow.AddHours(8);
                pTFunction.UPD_USRID = SignInProvider.Instance.User.ADAccount;

                if (mode == DataSaveMode.Add)
                {
                    this.PorDB.PTMenu.Add(pTMenu);
                    this.PorDB.PTFunction.Add(pTFunction);
                }
                else
                {
                    this.PorDB.Entry(pTMenu).State = EntityState.Modified;
                    this.PorDB.Entry(pTFunction).State = EntityState.Modified;
                }
                this.PorDB.SaveChanges();

                result = this.GetMenuDataByID(muID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            if (result == null)
                throw new Exception("儲存目錄後無法取得目錄資料");
            return result;
        }

        public bool SaveSubMenuData(DataSaveMode mode, SubMenuDataModel model)
        {
            bool isSuccess = true;
            string muID = model.SubMenuID;
            PTMenu pTMenu = null;
            PTFunction pTFunction = null;

            try
            {
                if (mode == DataSaveMode.Add)
                {
                    var chkHasMenu = this.GetMenuDataByID(muID);
                    if (chkHasMenu != null)
                        throw new Exception("已有MenuID為：" + muID + " 的選單，目錄名稱為：" + chkHasMenu.MenuName);
                    //menu
                    pTMenu = new PTMenu()
                    {
                        BUD_DTM = DateTime.UtcNow.AddHours(8),
                        BUD_USRID = SignInProvider.Instance.User.ADAccount
                    };
                    //function
                    pTFunction = new PTFunction()
                    {
                        BUD_DTM = DateTime.UtcNow.AddHours(8),
                        BUD_USRID = SignInProvider.Instance.User.ADAccount,
                    };
                }
                else if (mode == DataSaveMode.Edit)
                {
                    //menu
                    pTMenu = this.PorDB.PTMenu.Where(o => o.MUID == muID).FirstOrDefault();
                    if (pTMenu == null)
                        throw new Exception("無法取得選單主檔");
                    //function
                    pTFunction = this.GetProtorMentFunctionByID(muID);
                }
                else
                    throw new Exception("[儲存目錄]無法得知的儲存模式");

                pTMenu.MUID = model.SubMenuID;
                pTMenu.MU_NM = model.SubMenuName;
                pTMenu.MUICON = model.SubMenuIcon;
                pTMenu.ACT_FG = model.SubEnabled;
                pTMenu.MENU_FG = model.SubMenuEnabled;
                pTMenu.MUPID = model.SubParentMenuID;
                pTMenu.UPD_DTM = DateTime.UtcNow.AddHours(8);
                pTMenu.UPD_USRID = SignInProvider.Instance.User.ADAccount;

                pTFunction.MAP_MUID = muID;
                pTFunction.SORT_SEQ = model.SubMenuSort;
                pTFunction.FN_DIR = model.SubMenuPathController;
                pTFunction.FN_KEY = model.SubMenuPathAction;
                pTFunction.FN_LINK = string.Concat(model.SubMenuPathController, "/", model.SubMenuPathAction);
                pTFunction.UPD_DTM = DateTime.UtcNow.AddHours(8);
                pTFunction.UPD_USRID = SignInProvider.Instance.User.ADAccount;

                if (mode == DataSaveMode.Add)
                {
                    this.PorDB.PTMenu.Add(pTMenu);
                    this.PorDB.PTFunction.Add(pTFunction);
                }
                else
                {
                    this.PorDB.Entry(pTMenu).State = EntityState.Modified;
                    this.PorDB.Entry(pTFunction).State = EntityState.Modified;
                }
                this.PorDB.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isSuccess;
        }
        /// <summary>
        /// 取得選單內容檔
        /// </summary>
        /// <param name="muID"></param>
        /// <returns></returns>
        PTFunction GetProtorMentFunctionByID(string muID)
        {
            PTFunction pTFunction = this.PorDB.PTFunction.Where(o => o.MAP_MUID == muID).FirstOrDefault();
            if (pTFunction == null)
                throw new Exception("無法取得選單內容檔");
            return pTFunction;
        }

        /// <summary>
        /// 取得一筆目錄資料 已Join
        /// </summary>
        /// <param name="menuID"></param>
        /// <returns></returns>
        public MenuDataModel GetMenuDataByID(string menuID)
        {
            MenuDataModel menu = this.GetMenuData().Where(o => o.MenuID == menuID).FirstOrDefault();
            return menu;
        }

        /// <summary>
        /// 取所有選單資料 已Join
        /// </summary>
        /// <returns></returns>
        public List<MenuDataModel> GetMenuData()
        {
            var menuData = this.PorDB.PTMenu.Join(this.PorDB.PTFunction,
                    t1 => t1.MUID,
                    t2 => t2.MAP_MUID,
                    (menu, func) => new MenuDataModel()
                    {
                        MenuID = menu.MUID,
                        ParentMenuID = menu.MUPID,
                        MenuName = menu.MU_NM,
                        Enabled = (bool)menu.ACT_FG,
                        MenuEnabled = (bool)menu.MENU_FG,
                        MenuPathController = func.FN_DIR,
                        MenuPathAction = func.FN_KEY,
                        MenuPath = func.FN_LINK,
                        MenuSort = (int)func.SORT_SEQ,
                        MenuIcon = menu.MUICON
                    }).OrderBy(o => o.MenuID).ToList();
            return menuData;
        }
    }
}