using PagedList;
using Portal.Enums;
using Portal.Models.MenuModels.MenuListModels;
using Portal.Models.MenuModels.MenuSaveModels;
using Portal.Modules;
using PortalDataEntities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Repositories
{
    public class MenuRepository
    {
        private PORTALDB m_porDB = new PORTALDB();

        private PORTALDB PorDB { get { return this.m_porDB; } set { this.m_porDB = value; } }

        private MenuModule m_module = new MenuModule();
        private MenuModule Module { get { return this.m_module; } }

        public MenuListResultModel GetMenuList(MenuListFilterModel filterModel)
        {
            MenuListResultModel result = new MenuListResultModel();

            try
            {
                var menuList = this.PorDB.PTMenu.Select(s => new MenuListDataModel()
                {
                    MenuID = s.MUID,
                    ParentMenuID = s.MUPID,
                    MenuName = s.MU_NM,
                    MenuStatus = (bool)s.ACT_FG,
                    MenuDisplayStatus = (bool)s.MENU_FG,
                    BuildUserADAccount = s.BUD_USRID,
                    BuildDate = s.BUD_DTM,
                })
                .Where(
                    s => filterModel.FilterType == MenuListFilterType.MainMenuList ?
                    s.MenuID.Length == 1 :
                    s.ParentMenuID == filterModel.QueryString
                    )
                .ToList();

                menuList.ForEach(f =>
                {
                    f.SubMenuCount = this.MathSubMenuCount(f.MenuID);
                    f.BuildDateStr = f.BuildDate.To10CharString();
                    f.IsMade = this.CheckIsMadeSite(f.MenuID);
                });
                //Filter
                //menuList = this.ProcessFilterCondition(filterModel.FilterTargetEnum, filterModel.QueryString, empList);

                int currentPage = filterModel.CurrentPage;
                int pageSize = Convert.ToInt32(PublicStaticMethod.GetConfigAppSetting("DefaultPageSize"));
                result.Data = menuList.ToPagedList(currentPage, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// 確認是否已實作網站
        /// </summary>
        /// <param name="muID"></param>
        /// <returns></returns>
        private bool CheckIsMadeSite(string muID)
        {
            string msg = string.Empty;
            MenuDataModel menu = this.GetMenuByID(muID);
            string controllerName = menu.MenuPathController;
            string actionName = menu.MenuPathAction;
            var controllerNames = PublicStaticMethod.GetControllerNames();
            bool hasController = controllerNames.Any(a => a.Key == controllerName);
            bool hasAction = hasController ? controllerNames[controllerName].Any(a => a == actionName) : false;
            if (muID.Length == 1)
            {

                if (!hasController)
                    return false;
                else
                    return true;
            }

            if (!hasController || !hasAction)
                return false;
            return true;
        }

        /// <summary>
        /// 子目錄數量
        /// </summary>
        /// <param name="muID"></param>
        /// <returns></returns>
        private int MathSubMenuCount(string muID)
        {
            var count = this.PorDB.PTMenu.Where(o => o.MUPID == muID).Count();
            return count;
        }

        /// <summary>
        /// 取得選單資料
        /// </summary>
        /// <param name="muID"></param>
        /// <returns></returns>
        public MenuDataModel GetMenuByID(string muID)
        {
            MenuDataModel result = this.Module.GetMenuDataByID(muID);
            if (result == null)
                throw new Exception("無法取得選單資料");

            #region subMenuFilterCondition

            result.SubMenuData.Filter.QueryString = muID;
            result.SubMenuData.Filter.CurrentPage = 1;
            result.SubMenuData.Filter.FilterType = MenuListFilterType.SubMenuList;

            #endregion subMenuFilterCondition

            //子選單
            result.SubMenuData.Result = this.GetMenuList(result.SubMenuData.Filter);
            return result;
        }

        /// <summary>
        /// 儲存一筆目錄
        /// </summary>
        /// <param name="member"></param>
        public MenuDataModel MenuSave(MenuDataModel model, DataSaveMode mode)
        {
            MenuDataModel updateModel = null;
            MenuModule menuModule = new MenuModule();

            try
            {
                updateModel = menuModule.SaveMenuData(mode, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return updateModel;
        }

        /// <summary>
        /// 儲存一筆子目錄
        /// </summary>
        /// <param name="member"></param>
        public bool SubMenuSave(SubMenuDataModel model, DataSaveMode mode)
        {
            bool addSuccess = true;
            MenuModule menuModule = new MenuModule();

            try
            {
                addSuccess = menuModule.SaveSubMenuData(mode, model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return addSuccess;
        }

        /// <summary>
        /// 刪除一個目錄
        /// </summary>
        /// <param name="muID"></param>
        /// <returns></returns>
        public bool DeleteMenuByID(string muID)
        {
            Exception error = null;
            bool isDelSuccess = true;
            try
            {
                List<string> muIDList = this.PorDB.PTMenu.Where(o => o.MUID == muID || o.MUPID == muID).Select(s => s.MUID).ToList();
                //刪除角色對應檔
                var role = this.PorDB.PTRoleMenuMap.RemoveRange(this.PorDB.PTRoleMenuMap.Where(o => muIDList.Contains(o.MAP_MUID)));
                //刪除功能檔
                var func = this.PorDB.PTFunction.RemoveRange(this.PorDB.PTFunction.Where(o => muIDList.Contains(o.MAP_MUID)));
                //刪除主檔
                var mu = this.PorDB.PTMenu.RemoveRange(this.PorDB.PTMenu.Where(o => muIDList.Contains(o.MUID)));
                this.PorDB.SaveChanges();
            }
            catch (Exception ex)
            {
                error = ex;
            }
            finally
            {
                if (error != null)
                {
                    isDelSuccess = false;
                    throw error;
                }
                else
                    isDelSuccess = true;
            }
            return isDelSuccess;
        }
    }
}