using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;

namespace Portal.Repositories.api
{
    public class APIRepository
    {


        /// <summary>
        /// 取得資料成功
        /// </summary>
        /// <returns></returns>
        public static HttpResponseMessage ListRetrieved<TResource>(HttpRequestMessage request, TResource responseData)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            if (responseData == null)
                statusCode = HttpStatusCode.NotFound;
            return CreateDataResponse(request, statusCode, responseData);
        }

        /// <summary>
        /// 拒絕處理請求並回應訊息
        /// </summary>
        /// <param name="request"></param>
        /// <param name="message">訊息</param>
        /// <returns></returns>
        public static HttpResponseMessage ErrorRequest(HttpRequestMessage request, string message)
        {
            return request.CreateResponse(HttpStatusCode.Forbidden, message);
        }

        /// <summary>
        /// 輸出回傳結果 回傳型別HttpResponseMessage物件
        /// </summary>
        /// <typeparam name="T">輸出的物件類型</typeparam>
        /// <param name="request"></param>
        /// <param name="statusCode">HTTP狀態碼</param>
        /// <param name="data">輸出的資料</param>
        /// <returns></returns>
        internal static HttpResponseMessage CreateDataResponse<T>(HttpRequestMessage request, HttpStatusCode statusCode, T data)
        {
            HttpResponseMessage respMsg;
            MediaTypeFormatter mediaFormatter = null;
            mediaFormatter = new JsonMediaTypeFormatter();
            (mediaFormatter as JsonMediaTypeFormatter).SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            (mediaFormatter as JsonMediaTypeFormatter).SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            respMsg = request.CreateResponse(statusCode);
            respMsg.Content = new ObjectContent<T>(data, mediaFormatter);
            return respMsg;
        }
    }
}