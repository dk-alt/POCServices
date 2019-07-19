using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using POCLogger;

namespace POCDBAccess
{
   public static class Connection
    {

        public static IMongoDatabase GetMongoConnection()
        {
            IMongoDatabase database;
            MongoClient mongoClient;
            string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string dbName = ConfigurationManager.AppSettings["DBName"];
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(conString));
            clientSettings.UseSsl = true;
            clientSettings.SslSettings = new SslSettings() { CheckCertificateRevocation = false };
            clientSettings.VerifySslCertificate = false;
            //clientSettings.ReadPreference = new ReadPreference(ReadPreferenceMode.SecondaryPreferred);
            // Non SSL (Only for DEV)
            if (ConfigurationManager.AppSettings["environment"].ToLower().Equals("dev"))
                mongoClient = new MongoClient(conString);
            else // SSL For QA and Upper Environment use SSL Settings 
                mongoClient = new MongoClient(clientSettings);
            database = mongoClient.GetDatabase(dbName);
            Logger.Info("Connected to db:" + database.DatabaseNamespace.DatabaseName);
            return database;
        }


        public static IMongoDatabase GetSSLMongoConnection()
        {
            IMongoDatabase database;
            MongoClient mongoClient;
            string conString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string dbName = ConfigurationManager.AppSettings["DBName"];
            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(conString));
            clientSettings.UseSsl = true;
            var certificates1 = new[] { new X509Certificate("testcert.pfx", "password") };
            clientSettings.SslSettings = new SslSettings()
            {
                CheckCertificateRevocation = false,
                ClientCertificates = certificates1,
                EnabledSslProtocols = SslProtocols.Default
            };
            clientSettings.SslSettings.ClientCertificates = new List<X509Certificate>() { new X509Certificate("cert.perm") };
            clientSettings.SslSettings.EnabledSslProtocols = SslProtocols.Default;
            clientSettings.SslSettings.ClientCertificateSelectionCallback =
                (sender, host, certificates, certificate, issuers) => clientSettings.SslSettings.ClientCertificates.GetEnumerator().Current;
            clientSettings.SslSettings.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            clientSettings.VerifySslCertificate = true;

            //clientSettings.ReadPreference = new ReadPreference(ReadPreferenceMode.SecondaryPreferred);
            // Non SSL (Only for DEV)
            if (ConfigurationManager.AppSettings["environment"].ToLower().Equals("dev"))
                mongoClient = new MongoClient(conString);
            else // SSL For QA and Upper Environment use SSL Settings 
                mongoClient = new MongoClient(clientSettings);
            database = mongoClient.GetDatabase(dbName);
            Logger.Info("Connected to db:" + database.DatabaseNamespace.DatabaseName);
            return database;
        }
    }
}
