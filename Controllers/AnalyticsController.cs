using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CitygateApi.Models;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using System.Threading;
using Google.Apis.Util.Store;
using Google.Apis.Services;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Configuration;

namespace CitygateApi.Controllers
{
    public class AnalyticsController : ApiController
    {
        // OAuth client id
        //81713175964-advh7q4pag3hcqub30kueepin0kdn3qj.apps.googleusercontent.com

        //Oath client secret
        //29a-9XhpHW9wlrQ5hTrP1g6j
        
        public IHttpActionResult GetAnalytics()
        {
            Analytics an = TestServiceAccount(); 
            //var product = products.FirstOrDefault((p) => p.Id == id);
            if (an == null)
            {
                return NotFound();
            }
            return Ok(an);
        }

        // Service Account
        public Analytics TestServiceAccount()
        {
            string[] scopes = new string[] { AnalyticsService.Scope.Analytics }; // view and manage your Google Analytics data
            var keyFilePath = ConfigurationManager.AppSettings["keyFilePath"]; //@"c:\users\vivi\documents\visual studio 2015\Projects\CitygateApi\GoogleAnalyticsProject-5253532439c2.p12";    // Downloaded from https://console.developers.google.com

            var serviceAccountEmail = "81713175964-compute@developer.gserviceaccount.com";  // found https://console.developers.google.com
            //var serviceAccountEmail = "v.esquivias@gmail.com";
             // loading the Key file
             var certificate = new X509Certificate2(keyFilePath, "notasecret", X509KeyStorageFlags.Exportable);
            var credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(serviceAccountEmail)
            {
                Scopes = scopes
            }.FromCertificate(certificate));

            // Analytics Service
            var service = new AnalyticsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "Analytics API Sample",
            });

            // Analytics Core
            DataResource.GaResource.GetRequest request = service.Data.Ga.Get("ga:40204906", "2017-01-23", "2017-01-29", "ga:sessions, ga:pageviews");
            request.MaxResults = 1000;
            Google.Apis.Analytics.v3.Data.GaData result = request.Execute();
            //List<List<String>> t = (List<List<String>>)result.Rows; //new List<List<string>>(); 
            foreach (var headers in result.ColumnHeaders)
            {
                string name = headers.Name;
                string ctype = headers.ColumnType;
                string dtype = headers.DataType;
            }

            int sessions = int.Parse(result.Rows[0][0]);
            int pageviews = int.Parse(result.Rows[0][1]);

            Analytics an = new Analytics
            {
                Id = "ASD123",
                Name = "2017-01-23 - 2017-01-29",
                Sessions = sessions,
                PageViews = pageviews
            };

            return an; 

            /*foreach (var row in result.Rows)
            {
                foreach (string col in row)
                {
                    string column = col; 
                }
            }*/

        }

        public void Test()
        {
            var url = ConfigurationManager.AppSettings["ServiceProviderUrl"];
            string[] scopes = new string[] {
            AnalyticsService.Scope.Analytics,               // view and manage your Google Analytics data
            //AnalyticsService.Scope.AnalyticsEdit,           // Edit and manage Google Analytics Account
            //AnalyticsService.Scope.AnalyticsManageUsers,    // Edit and manage Google Analytics Users
            AnalyticsService.Scope.AnalyticsReadonly};      // View Google Analytics Data
            
            var clientId = "81713175964-a4tgd0ln1sks74afhvabtrdmsncnpgcf.apps.googleusercontent.com";      // From https://console.developers.google.com
            var clientSecret = "4cyJvVRPMqRUU2usNz-W8txg";          // From https://console.developers.google.com
            // here is where we Request the user to give us access, or use the Refresh Token that was previously stored in %AppData%
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            scopes,
            Environment.UserName,
            CancellationToken.None,
            new FileDataStore("Daimto.GoogleAnalytics.Auth.Store")).Result;
        }
    }
}
