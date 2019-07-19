using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Unity;
using POCDBAccess;
using System.Web.Http.ExceptionHandling;
using System.Configuration;
using System.Web.Http.Cors;

namespace POCServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            //Web API Configuration & services for unity dependency resolver
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IMongoConnect, MongoConnect>();
            config.DependencyResolver = new Filters.UnityResolver(container);
            // Web API routes
            config.MapHttpAttributeRoutes();
            config.EnableCors(SetCorsConfig());
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
          //  config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
           
        }

        private static ICorsPolicyProvider SetCorsConfig()
        {
            string origins = ConfigurationManager.AppSettings["Origins"];
            string headers = ConfigurationManager.AppSettings["Headers"];
            string methods = ConfigurationManager.AppSettings["Methods"];
            string exposedHeaders = ConfigurationManager.AppSettings["ExposedHeaders"];
            var corsConfig = new EnableCorsAttribute(origins, headers, methods, exposedHeaders);

            corsConfig.SupportsCredentials = true;
            corsConfig.PreflightMaxAge = 3600;

            return corsConfig;

        }
    }
}
