using Microsoft.Owin;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;
using System;
using System.Configuration;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using POCLogger;

namespace POCServices
{
    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Tenant = ConfigurationManager.AppSettings["ida:Tenant"],
                    TokenValidationParameters = new TokenValidationParameters { SaveSigninToken = true, ValidAudience = ConfigurationManager.AppSettings["ida:Audience"] }
                });

            try
            {
                app.UseActiveDirectoryFederationServicesBearerAuthentication(
                  new ActiveDirectoryFederationServicesBearerAuthenticationOptions
                  {
                      MetadataEndpoint = ConfigurationManager.AppSettings["ida:AdfsMetadataEndpoint"],
                      TokenValidationParameters = new TokenValidationParameters()
                      {
                          SaveSigninToken = true,
                          ValidAudience = ConfigurationManager.AppSettings["ida:Audience"],
                          //     ValidIssuer = ConfigurationManager.AppSettings["ida:Issuer"]
                      }
                  }
              );
            }
            catch (Exception ex)
            {

                Logger.Error("Authentication Error ADFS", ex);
            }
        }
    }
}