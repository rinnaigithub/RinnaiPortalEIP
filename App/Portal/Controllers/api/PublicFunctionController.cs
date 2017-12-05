using Portal.Repositories.api;
using System;
using System.Net.Http;
using System.Web.Http;

namespace Portal.Controllers.api
{
    public class PublicFunctionController : ApiController
    {
        private PublicFunctionRepository m_publicFunctionRepository = new PublicFunctionRepository();
        private PublicFunctionRepository PublicRepository { get { return this.m_publicFunctionRepository; } }

        private APIRepository m_aPIRepository = new APIRepository();
        private APIRepository Repository { get { return this.m_aPIRepository; } }

        /// <summary>
        /// 取得志元員工資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetSmartManAllEmployeeDataJson()
        {
            try
            {
                var empList = this.PublicRepository.GetSmartManEmpPartialData();
                return APIRepository.ListRetrieved(this.Request, empList);
            }
            catch (Exception ex)
            {
                return APIRepository.ErrorRequest(this.Request, ex.Message);
            }
        }

        /// <summary>
        /// 取得部門資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetDepartmentInformationJson()
        {
            try
            {
                var depList = this.PublicRepository.GetDepartmentPartialData();
                return APIRepository.ListRetrieved(this.Request, depList);
            }
            catch (Exception ex)
            {
                return APIRepository.ErrorRequest(this.Request, ex.Message);
            }
        }

        /// <summary>
        /// 取得角色資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetRoleInformationJson()
        {
            try
            {
                var roleList = this.PublicRepository.GetRolePartialData();
                return APIRepository.ListRetrieved(this.Request, roleList);
            }
            catch (Exception ex)
            {
                return APIRepository.ErrorRequest(this.Request, ex.Message);
            }
        }

        /// <summary>
        /// 取得目錄資料
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public HttpResponseMessage GetMenuInformationJson()
        {
            try
            {
                var menuList = this.PublicRepository.GetMenuPartialData();
                return APIRepository.ListRetrieved(this.Request, menuList);
            }
            catch (Exception ex)
            {
                return APIRepository.ErrorRequest(this.Request, ex.Message);
            }
        }

    }
}