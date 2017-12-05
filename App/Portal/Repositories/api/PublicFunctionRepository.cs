using Portal.Models.api;
using PortalDataEntities.Entities;
using SmartManDataEntities.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Portal.Repositories.api
{
    public class PublicFunctionRepository
    {
        private PORTALDB m_portalDB = new PORTALDB();
        internal PORTALDB PortalDB { get { return this.m_portalDB; } }

        private HRISDB m_smartManDB = new HRISDB();
        internal HRISDB SmartManDB { get { return this.m_smartManDB; } }

        #region Entity Proto Data

        /// <summary>
        /// 取得志元所有員工資料
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SmartManDataEntities.Entities.EMPLOYEE> GetSmartManProtoAllEmployeeData()
        {
            var result = this.SmartManDB.EMPLOYEE.ToList();
            return result;
        }

        /// <summary>
        /// 取得所有部門資料資料庫原型欄位
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Department> GetProtoDepartmentData()
        {
            var result = this.PortalDB.Department.ToList();
            return result;
        }

        /// <summary>
        /// 取得所有角色資料庫原型欄位
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PTRole> GetProtoRoleData()
        {
            var result = this.PortalDB.PTRole.ToList();
            return result;
        }

        #endregion Entity Proto Data

        /// <summary>
        /// 取得所有部門資料 包含所有明細
        /// </summary>
        /// <returns></returns>
        public List<DepartmentDataModel> GetDepartmentData()
        {
            List<DepartmentDataModel> result = this.PortalDB.Department.Select(o => new DepartmentDataModel()
            {
                DepartmentID = o.DepartmentID,
                DepartmentName = o.DepartmentName,
                DeptLevel = o.DepartmentLevel,
                Disabled = (bool)o.Disabled,
                LeaderEmpID = o.ChiefID_FK,
                DisabledDate = o.DisabledDate,
                BuildADAcount = o.Creator,
                BuildDate = o.CreateDate,
                UpdateADAcount = o.Modifier,
                UpdateDate = o.ModifyDate,
                UpperDepartmentID = o.UpperDepartmentID
            }).Where(s => s.Disabled == false).ToList();
            return result;
        }

        #region PartialData

        public SmartManEmpPartialModel GetSmartManEmpPartialData()
        {
            SmartManEmpPartialModel empPartialModel = new SmartManEmpPartialModel();
            var potyEmpData = this.GetSmartManProtoAllEmployeeData();
            //QUITDATE == 0 在職人員
            potyEmpData = potyEmpData.Where(o => o.QUITDATE == 0).ToList();

            List<Portal.Models.api.SmartManEmpPartialModel.EmployeePartialDataModel> result = potyEmpData
                .Select(o => new Portal.Models.api.SmartManEmpPartialModel.EmployeePartialDataModel()
            {
                id = o.EMPLOYECD,
                text = o.EMPLOYECD + " - " + o.CHINESENAME,
            }).ToList();
            empPartialModel.results.AddRange(result);
            return empPartialModel;
        }

        /// <summary>
        /// 取得所有部門資料 只有部門ID與部門名稱
        /// </summary>
        /// <returns></returns>
        public RolePartialModel GetRolePartialData()
        {
            RolePartialModel rolePartialModel = new RolePartialModel();
            var potyRoleData = this.GetProtoRoleData();
            potyRoleData = potyRoleData.Where(o => o.DEL_FG == false).ToList();

            List<Portal.Models.api.RolePartialModel.RolePartialDataModel> result = potyRoleData.Select(o => new Portal.Models.api.RolePartialModel.RolePartialDataModel()
            {
                id = o.ID,
                text = o.ROLE_NM,
            }).ToList();
            rolePartialModel.results.AddRange(result);
            return rolePartialModel;
        }

        /// <summary>
        /// 取得所有部門資料 只有部門ID與部門名稱
        /// </summary>
        /// <returns></returns>
        public DepartmentPartialModel GetDepartmentPartialData()
        {
            DepartmentPartialModel depPartialModel = new DepartmentPartialModel();
            var potyDepData = this.GetProtoDepartmentData();
            potyDepData = potyDepData.Where(o => o.Disabled == false).ToList();

            List<DepartmentPartialDataModel> result = potyDepData.Select(o => new DepartmentPartialDataModel()
            {
                id = o.DepartmentID,
                text = o.DepartmentName,
            }).ToList();
            depPartialModel.results.AddRange(result);
            return depPartialModel;
        }

        /// <summary>
        /// 取得所有部門資料 只有部門ID與部門名稱
        /// </summary>
        /// <returns></returns>
        public MenuPartialModel GetMenuPartialData()
        {
            IEnumerable<PTMenu> filterMenu = this.PortalDB.PTMenu.Where(o => o.ACT_FG == true).ToList();
            MenuPartialModel menuPartialModel = new MenuPartialModel();
            var result = filterMenu.Join(this.PortalDB.PTFunction,
                          t1 => t1.MUID,
                          t2 => t2.MAP_MUID,
                          (menu, func) => new MenuPartialModel.MenuPartialDataModel()
                          {
                              id = menu.MUID,
                              text = menu.MU_NM
                          })
                          .ToList();
            menuPartialModel.results.AddRange(result);
            return menuPartialModel;
        }


        #endregion PartialData
    }
}