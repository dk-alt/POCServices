using POCLogger;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace POCServices.Filters
{
    public class LogActionFilter : ActionFilterAttribute
    {

        private string _logData;
        readonly bool _serviceLogs = false;
        readonly bool writeResponse = false;
        readonly bool writeRequest = false;

        //Getting configuration values for action filter to add/skip this fliter 
        public LogActionFilter()
        {

            try
            {
                _serviceLogs = Convert.ToBoolean(ConfigurationManager.AppSettings["ServiceLogs"]);
                writeResponse = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteResponse"]);
                writeRequest = Convert.ToBoolean(ConfigurationManager.AppSettings["WriteRequest"]);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while getting exception-filter config values", ex);
            }


        }


        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (_serviceLogs)
            {
                try
                {

                    base.OnActionExecuted(actionExecutedContext);
                    Stopwatch stopwatch = (Stopwatch)actionExecutedContext.Request.Properties[actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName];
                    stopwatch.Stop();
                    var elapsedTime = stopwatch.ElapsedMilliseconds;
                    if (actionExecutedContext.Response != null)
                    {
                        //if write response config is true then only write the response else skip
                        var response = writeResponse ? actionExecutedContext.Response.Content.ReadAsStringAsync().Result : null;
                        _logData += Environment.NewLine + "RESPONSE_BODY : " + response ?? "Null";
                        Logger.Info(_logData);
                        Logger.Info("Controller:: " + actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName + "_API Elapsed, Time(ms):" + elapsedTime.ToString());
                        Logger.Info("ActionMethod:: " + actionExecutedContext.ActionContext.ActionDescriptor.ActionName + "_API, Elapsed Time(ms):" + elapsedTime.ToString());
                    }
                    stopwatch.Reset();
                    _logData = string.Empty;
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception Occurred", ex);
                }
            }
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (_serviceLogs)
            {
                try
                {
                    base.OnActionExecuting(actionContext);
                    string rawRequest = null;
                    //if write request is false skip writing request 
                    if (actionContext.Request.Content != null && !actionContext.Request.Content.IsMimeMultipartContent() && writeRequest)
                    {
                        using (var stream = new StreamReader(actionContext.Request.Content.ReadAsStreamAsync().Result))
                        {
                            stream.BaseStream.Position = 0;
                            rawRequest = stream.ReadToEnd();
                        }
                    }

                    var stopWatch = new Stopwatch();
                    stopWatch.Start();
                    actionContext.Request.Properties[actionContext.ControllerContext.ControllerDescriptor.ControllerName] = stopWatch;

                    _logData = string.Empty;
                    _logData += Environment.NewLine + "REQUEST_URL : " + actionContext.Request.RequestUri.AbsoluteUri;
                    _logData += Environment.NewLine + "REQUEST_TYPE : " + actionContext.Request.Method.Method;
                    _logData += Environment.NewLine + "REQUEST_BODY : " + rawRequest;
                }
                catch (Exception ex)
                {
                    Logger.Error("Exception Occurred", ex);
                }
            }
        }

    }
}