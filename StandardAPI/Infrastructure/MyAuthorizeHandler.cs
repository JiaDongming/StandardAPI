using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Security.Claims;
using System.Net.Http.Headers;

namespace StandardAPI.Infrastructure
{
    /// <summary>
    /// HTTP Message handler： 消息拦截器
    /// 
    /// </summary>
    public class MyAuthorizeHandler:DelegatingHandler
    {
        /// <summary>
        /// 实现DelegatingHandler的SendAsync方法
        /// </summary>
        /// <param name="request">Http请求</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //1. 根据WebApiConfig里添加了该拦截器MyAuthorizeHandler后，API请求在到达HttpServer后，先进入了该消息拦截器的该方法

            //2. 根据收到的Request进行处理认证，一般分以下步骤
            /*处理步骤
             * 1. 接收传入的HttpRequestMessage对象 request
             * 2. 根据需要支持的情况，以几种方式处理request里传入的参数获取token，比如
             *     Use URL query string to pass the token：   https://abc.com/api/project?projectid=181&token=2A693723-2ACC-4493-BF9C-17FDA531643A
             *     Use the Authorization header to pass the token：   Authorization: bearer 2A693723-2ACC-4493-BF9C-17FDA531643A
             *     User cookie named 'UserToken' to pass the token：    Cookie: UserToken=2A693723-2ACC-4493-BF9C-17FDA531643A;
             *  3. 判断获取的token是否有效,如果有效，则设置Thread.CurrentPrincipal为IPrincipal,并且设置HttpContext.Current.User 为IPrincipal对象
             *  4. 在WebApiConfig里需要注册该消息拦截器才能起效   config.MessageHandlers.Add(new MyAuthorizeHandler());
             *  5. 在指定位置，比如Controller处增加特性起效  [Authorize]
             */

            Authenticate(request);
            //int userId = 611;
            //ClaimsIdentity identity = new ClaimsIdentity("Bearer");
            //identity.AddClaim(new Claim("userid", userId.ToString()));
            //IPrincipal principal = new GenericPrincipal(identity, null);
            //Thread.CurrentPrincipal = principal;
            //if (HttpContext.Current != null)
            //{
            //    HttpContext.Current.User = principal;
            //}
            return base.SendAsync(request, cancellationToken);
        }

        /// <summary>
        /// 2. 根据需要支持的情况，以几种方式处理request里传入的参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private string RetrieveToken(HttpRequestMessage request)
        {
            string token = string.Empty;

            // From Query string
            var paramValues = request.RequestUri.ParseQueryString().GetValues("token");
            if (paramValues != null)
            {
                token = paramValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
            }

            // From Authorization header
            AuthenticationHeaderValue auth = request.Headers.Authorization;
            if (auth != null && string.Equals(auth.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase))
            {
                token = auth.Parameter;
                if (!string.IsNullOrEmpty(token))
                    return token;
            }

            // From UserToken request header
            IEnumerable<string> headerValues = null;
            if (request.Headers.TryGetValues("UserToken", out headerValues) && headerValues != null)
            {
                token = headerValues.FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                    return token;
            }

            // From UserToken Cookie
            var cookie = request.Headers.GetCookies("UserToken").FirstOrDefault();
            if (cookie != null)
            {
                token = cookie["UserToken"].Value;
                if (!string.IsNullOrEmpty(token))
                    return token;
            }
            return token;
        }


        private void Authenticate(HttpRequestMessage request)
        {
            string token = RetrieveToken(request);
            if (!string.IsNullOrEmpty(token))
            {
                int userId = 611;
                //int userId =ApiManager.Instance.VerifyTokenUser(token);
                //if (userId > 0)
                if(token=="123456")
                {
                    ClaimsIdentity identity = new ClaimsIdentity("Bearer");
                    identity.AddClaim(new Claim("userid", userId.ToString()));
                    IPrincipal principal = new GenericPrincipal(identity, null);
                    SetPrincipal(principal);

                    CurrentUserContext.Token = token;
                    CurrentUserContext.UserId = userId;
                }
                else if(token=="ABCDEF")
               // else if (IntegrationLogic.VerifyIntegrationAppKey(token))
                {
                    //userId =  LoginManage.INTEGRATION_USER_ID;
                    userId = 0;

                    ClaimsIdentity identity = new ClaimsIdentity("Bearer");
                    identity.AddClaim(new Claim("userid", userId.ToString()));
                    IPrincipal principal = new GenericPrincipal(identity, null);
                    SetPrincipal(principal);

                    CurrentUserContext.Token = token;
                    CurrentUserContext.UserId = userId;
                }
            }
        }

        private static void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }

    }
    /// <summary>
    /// 
    /// </summary>
    public class CurrentUserContext
    {
  
        [ThreadStatic]
        public static string Token;
        [ThreadStatic]
        public static int UserId;
        [ThreadStatic]
        public static int LanguageId;
        [ThreadStatic]
        public static TimeSpan? UserTimeZoneOffsetInCookie;
        [ThreadStatic]
        public static TimeSpan UserTimeZoneOffset;
        [ThreadStatic]
        public static int UserTimeZone;

        public static int DBTimeZone;
        public static TimeSpan TimeZoneOffsetInDB;



        [ThreadStatic]
        public static Dictionary<int, int> UserTimezones;
    }
}