using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Middleware
{
    public class CustomAuthMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly string _authToken;
        public CustomAuthMiddleware(RequestDelegate next, string authToken)
        {
            _next = next;
            _authToken = authToken;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            ////檢查是否為公開的Router
            //if (!context.Request.Path.StartsWithSegments("/api/XinWeb/v1/HistoryTest"))
            //{
            //}
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Bearer"))
            {
                string userToken = authHeader.Substring("Bearer ".Length).Trim();

                try
                {
                    // 驗證Token
                    var claimsPrincipal = ValidateToken(userToken);
                    context.User = claimsPrincipal;
                }
                catch
                {
                    throw new AuthenticationException("驗證未通過" + " | 客戶端Token:" + $"\"{userToken}\"");
                }
            }
            else
            {
                throw new AuthenticationException("未帶入任何驗證參數");
            }
            
            await _next(context);
        }

        private ClaimsPrincipal ValidateToken(string userToken)
        {
            if (userToken.Equals(_authToken))
            {
                var claimsIdentity = new ClaimsIdentity(new Claim[] { new Claim("user", userToken) }, "Basic");
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                return claimsPrincipal;
            }
            else
            {
                throw new AuthenticationException("驗證未通過" + " | 客戶端Token:" + $"\"{userToken}\"");
            }
        }
    }
}
