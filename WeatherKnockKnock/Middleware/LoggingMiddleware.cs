using MySqlConnector;
using Newtonsoft.Json;
using System.Net;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Util;

namespace Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            string requestBody = "";
            string requestIP = "";
            string requestPath = "";
            DateTimeOffset startTime = DateTimeOffset.UtcNow.LocalDateTime;
            var originalResponseBody = context.Response.Body;
            using var newResponseBody = new MemoryStream();
            context.Response.Body = newResponseBody;

            try
            {
                #region 紀錄 Request
                requestPath = context.Request.Path;
                long startTimeStamp = startTime.ToUnixTimeMilliseconds();
                var iPAddress = context.Connection.RemoteIpAddress;
                if (iPAddress != null)
                {
                    requestIP = iPAddress.MapToIPv4().ToString();
                }
                requestBody = context.Request.Path;
                HttpRequest httpRequest = context.Request;
                requestBody = Regex.Replace(await ReadBodyAsync(httpRequest), @"[\r\n]", "");
                //string requestMsg = Global.GetLogFormat(startTimeStamp, requestIP, requestPath, requestBody, 0);
                //if (Global.RequestYN)
                //{
                //    Global.Logger.Info(requestMsg);
                //}
                #endregion

                await _next(context);
            }
            catch (MySqlException mysqlException)
            {
                #region 處理MySQL錯誤

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // 建立錯誤回應物件
                DateTimeOffset now = DateTimeOffset.UtcNow.LocalDateTime;
                string nowDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                long timeStamp = now.ToUnixTimeMilliseconds();

                #region 處理回傳錯誤
                var result = new
                {
                    total = (long)0,
                    data = new List<object>()
                };
                //ApiResponse apiResponse = new ApiResponse
                //{
                //    success = false,
                //    message = "Database Error",
                //    messageType = MessageType.MdbError,
                //    time = nowDate,
                //    resultData = result
                //};
                //string jsonResponse = JsonConvert.SerializeObject(apiResponse);
                //var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(apiResponse));
                string content = $"{JsonConvert.DeserializeObject(requestBody)} | Error:{mysqlException.Message}";
                DateTime endTime = DateTime.Now;
                TimeSpan elapsedTime = endTime - startTime;
                double useTime = elapsedTime.TotalMilliseconds;
                //string errorMsg = Global.GetLogFormat(timeStamp, requestIP, requestPath, content, useTime);
                #endregion

                // 寫下錯誤
                //Global.Logger.Error(errorMsg);
                // 送出信件
                //await LimitExtension.發送MailAsync(requestPath, mysqlException.Message);

                //await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                #endregion
            }
            catch (AuthenticationException authenticationException)
            {
                #region 處理驗證錯誤

                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                // 建立錯誤回應物件
                DateTimeOffset now = DateTimeOffset.UtcNow.LocalDateTime;
                string nowDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                long timeStamp = now.ToUnixTimeMilliseconds();

                #region 處理回傳錯誤
                var result = new
                {
                    total = (long)0,
                    data = new List<object>()
                };
                //ApiResponse apiResponse = new ApiResponse
                //{
                //    success = false,
                //    message = "未填入token或驗證失敗",
                //    messageType = MessageType.Mexception,
                //    time = nowDate,
                //    resultData = result
                //};
                //string jsonResponse = JsonConvert.SerializeObject(apiResponse);
                //var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(apiResponse));
                string content = $"{JsonConvert.DeserializeObject(requestBody)} | Error:{authenticationException.Message}";
                DateTime endTime = DateTime.Now;
                TimeSpan elapsedTime = endTime - startTime;
                double useTime = elapsedTime.TotalMilliseconds;
                //string errorMsg = Global.GetLogFormat(timeStamp, requestIP, requestPath, content, useTime);
                #endregion

                // 寫下錯誤
                //Global.Logger.Error(errorMsg);

                //await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                #endregion
            }
            catch (KnownException knownException)
            {
                #region 處理已知的錯誤

                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Response.ContentType = "application/json";

                // 建立錯誤回應物件
                DateTimeOffset now = DateTimeOffset.UtcNow.LocalDateTime;
                string nowDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                long timeStamp = now.ToUnixTimeMilliseconds();

                #region 處理回傳錯誤
                var result = new
                {
                    total = (long)0,
                    data = new List<object>()
                };
                //ApiResponse apiResponse = new ApiResponse
                //{
                //    success = false,
                //    message = knownException.Message,
                //    messageType = MessageType.Mexception,
                //    time = nowDate,
                //    resultData = result
                //};
                //string jsonResponse = JsonConvert.SerializeObject(apiResponse);
                //var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(apiResponse));
                string content = $"{JsonConvert.DeserializeObject(requestBody)} | Error:{knownException.Message}";
                DateTime endTime = DateTime.Now;
                TimeSpan elapsedTime = endTime - startTime;
                double useTime = elapsedTime.TotalMilliseconds;
                //string errorMsg = Global.GetLogFormat(timeStamp, requestIP, requestPath, content, useTime);
                #endregion

                // 寫下錯誤
                //Global.Logger.Error(errorMsg);

                //await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                #endregion
            }
            
            catch (Exception exception)
            {
                #region 處理未知錯誤
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // 建立錯誤回應物件
                DateTimeOffset now = DateTimeOffset.UtcNow.LocalDateTime;
                string nowDate = now.ToString("yyyy-MM-dd HH:mm:ss");
                long timeStamp = now.ToUnixTimeMilliseconds();

                #region 處理回傳錯誤
                var result = new
                {
                    total = (long)0,
                    data = new List<object>()
                };
                //ApiResponse apiResponse = new ApiResponse
                //{
                //    success = false,
                //    message = "Server Error",
                //    messageType = MessageType.Mexception,
                //    time = nowDate,
                //    resultData = result
                //};
                //string jsonResponse = JsonConvert.SerializeObject(apiResponse);
                //var responseBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(apiResponse));
                string content = $"{JsonConvert.DeserializeObject(requestBody)} | Error:{exception.Message}";
                DateTime endTime = DateTime.Now;
                TimeSpan elapsedTime = endTime - startTime;
                double useTime = elapsedTime.TotalMilliseconds;
                //string errorMsg = Global.GetLogFormat(timeStamp, requestIP, requestPath, content, useTime);
                #endregion

                // 寫下錯誤
                //Global.Logger.Error(errorMsg);

                //await context.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                #endregion
            }
            finally
            {
                #region 紀錄 Response

                //if (Global.ResponseYN)
                //{
                    DateTimeOffset endTime = DateTimeOffset.UtcNow.LocalDateTime;
                    long endTimeStamp = endTime.ToUnixTimeMilliseconds();
                    double useTime = (endTime - startTime).TotalMilliseconds;

                    //處理HttpResponse的流
                    var httpResponse = await ReadBodyAsync(context.Response);
                    await newResponseBody.CopyToAsync(originalResponseBody);
                    var match = Regex.Match(httpResponse, @"""total""\s*:\s*(\d+)");
                    string totalValue = "";
                    if (match.Success)
                    {
                        totalValue = " {\"total\": " + $"\"{int.Parse(match.Groups[1].Value)}\"" + "}";
                    }
                    //string responseMsg = Global.GetLogFormat(endTimeStamp, requestIP, requestPath, totalValue, useTime);
                    //Global.Logger.Info(responseMsg);
                //}
                else
                {
                    context.Response.Body.Seek(0, SeekOrigin.Begin);
                    await newResponseBody.CopyToAsync(originalResponseBody);
                }
                #endregion
            }
        }

        #region 讀取HttpBody的方法
        private async Task<string> ReadBodyAsync(HttpRequest request)
        {
            request.EnableBuffering();
            using var streamReader = new StreamReader(request.Body, leaveOpen: true);
            string requestBody = await streamReader.ReadToEndAsync();
            request.Body.Position = 0;
            return requestBody;
        }
        private async Task<string> ReadBodyAsync(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return responseBody;
        }
        #endregion
    }
}
