
using System.Configuration;

namespace EAccess.App_Code
{
    public static class Configuration
    {
        // Caches the Connection String
        private readonly static string dbConnectionString;
        // Caches the Data Provider Name
        private readonly static string dbProviderName;
        // Store Name of website 
        private readonly static string siteName;
        // Store Title e.g. "eAccess" 
        private readonly static string siteTitle;
        // Store Copy Right Date And Company e.g. "2009 ELM Global Logistics"
        private readonly static string copyRight;

        // Reporting
        // Caches the DataBase Name for Reports
        private readonly static string dbName4Reports;
        // Caches Report Path
        private readonly static string path4Reports;

        // Error Logging
        private readonly static string webLogging;
        private readonly static bool errorLogEmail;


        // Initialize various properties in the constructor 
        static Configuration()
        {
            // web.config <connectionStrings>
            dbConnectionString = ConfigurationManager.ConnectionStrings["TRIDENTConnectionString"].ConnectionString;
            dbProviderName = ConfigurationManager.ConnectionStrings["TRIDENTConnectionString"].ProviderName;

            // web.config <appSettings>
            siteName = ConfigurationManager.AppSettings["SiteName"];
            siteTitle = ConfigurationManager.AppSettings["SiteTitle"];
            copyRight = ConfigurationManager.AppSettings["CopyRight"];

            // Reporting web.config <appSettings> Reporting
            dbName4Reports = ConfigurationManager.AppSettings["ReportDataBase"];
            path4Reports = ConfigurationManager.AppSettings["ReportPath"];

            // Error Logging web.config <appSettings>
            webLogging = ConfigurationManager.AppSettings["WebLogging"];
            errorLogEmail = bool.Parse(ConfigurationManager.AppSettings["ErrorLogEmail"]);


        }

        // Returns the connection string for the DataBase
        public static string DbConnectionString
        {
            get
            {
                return dbConnectionString;
            }
        }

        // Returns the data provider name 
        public static string DbProviderName
        {
            get
            {
                return dbProviderName;
            }
        }

        // Returns Website Name
        public static string SiteName
        {
            get
            {
                return siteName;
            }
        }

        // Returns Site's Title
        public static string SiteTitle
        {
            get
            {
                return siteTitle;
            }
        }

        // Returns CopyRight
        public static string CopyRight
        {
            get
            {
                return copyRight;
            }
        }

        // Returns Reports DataBase 
        public static string ReportDataBase
        {
            get
            {
                return dbName4Reports;
            }
        }


        // Returns Reports Path
        public static string ReportPath
        {
            get
            {
                return path4Reports;
            }
        }

        // Returns WebLogging File Path Plus FileName
        public static string WebLogging
        {
            get
            {
                return webLogging;
            }
        }


        // Returns ErrorLogEmail 
        public static bool ErrorLogEmail
        {
            get
            {
                return errorLogEmail;
            }
        }



    }


}