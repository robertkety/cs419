using CommandLine;
using CommandLine.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Storage.Models;
using Microsoft.WindowsAzure.Management.WebSites.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Web.Deployment;
using System.Net.FtpClient;
using Microsoft.WindowsAzure.Management.Sql;
using Microsoft.WindowsAzure.Management.Sql.Models;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using System.Net;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace CRRDirectoryInstall
{
    class Program
    {
        public static string CertificatePath = "/Certificates/cs419cert.cer";
        public static string StorageAccountName = "crr0";
        public static string StorageAccessKey = "";
        public static string WebSpaceName = "";
        public static string WebApiName = "";
        public static string WebInterfaceName = "";
        public static string ApiPath = "/WebAPI/";
        public static string WebManagementAppPath = "/WebManagementApp/";
        public static string DatabaseName = "";
        public static string DBUserName = "adminUser";
        public static string DBPassword = "GoBeavs247";
        public static string DBServerName = "";
        public static string DBRuleName = "CRRDirectoryInstall";

        // Thanks to http://commandline.codeplex.com
        class Options
        {
            [Option('s', "subscription", Required = true, HelpText = "Azure Subscription ID")]
            public string SubscriptionId { get; set; }

            [Option('v', "verbose", DefaultValue = false,
              HelpText = "Displays details of the upload process")]
            public bool Verbose { get; set; }

            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpOption]
            public string GetUsage()
            {
                return HelpText.AutoBuild(this,
                  (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            }
        }

        static void Main(string[] Args)
        {
            AsyncContext.Run(() => MainAsync(Args));    //Thanks! http://stackoverflow.com/questions/9208921/async-on-main-method-of-console-app
        }
        
        static async void MainAsync(string[] Args)
        {
            var Options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(Args, Options))
            {
                string path = System.IO.Directory.GetCurrentDirectory() + "/StorageTables/";
                bool Verbose = Options.Verbose;
#if DEBUG
                Verbose = true;
#endif

                try
                {
                    SubscriptionCloudCredentials Credentials = getCredentials(Options.SubscriptionId);
                    
                    //Storage Account
                    Console.WriteLine("Connecting to Windows Azure and creating new Azure Storage Account\nThis will take a few minutes so I'll create your database while we wait.");
                    var ResponseTask = CreateStorageAccountName(Credentials);

                    //Database
                    Console.WriteLine("\nCreating Database");
                    DatabaseName = CreateDatabase(Credentials, StorageAccountName + "-db");
                    Console.WriteLine("Database Created");
                    Console.WriteLine("\nPopulating Database Schema and Data");
                    PopulateDatabase(Credentials, DatabaseName);
                    Console.WriteLine("Database populated");

                    //Add Firewall Rule
                    AddFirewallRule(Credentials);
                    DeleteFirewallRule(Credentials);

                    Console.WriteLine("Still waiting on Azure Storage Account Creation");
                    if ((await ResponseTask).StatusCode.ToString().Contains("OK"))
                    {
                        Console.WriteLine("Storage Account Created: " + StorageAccountName);
                    }
                    else
                    {
                        throw new CloudException("Storage Account Failed: " + StorageAccountName);
                    }
                    Console.WriteLine("\nRequesting Secondary Key for new Azure Storage Account");
                    StorageAccessKey = GetStorageAccessKey(Credentials);
                    Console.WriteLine("Key Retrieved");
                    Console.WriteLine("\nPopulating Storage Tables");
                    PopulateStorageAccount(Credentials, path, Verbose);

                    //Web API
                    Console.WriteLine("\nDeploying Web API");
                    ConfigureApp(ApiPath);
                    WebApiName = CreateWebApp(Credentials, StorageAccountName + "-webapi");
                    DeployWebSite(Credentials, ApiPath, WebApiName, Verbose);
                    Console.WriteLine("Web API Deployed");

                    //Web Management Portal
                    Console.WriteLine("\nDeploying Web Management Portal");
                    ConfigureApp(WebManagementAppPath);
                    WebInterfaceName = CreateWebApp(Credentials, StorageAccountName + "-management");
                    DeployWebSite(Credentials, WebManagementAppPath, WebInterfaceName, Verbose);
                    Console.WriteLine("Web Management Portal Deployed\n");
                    
                    Console.WriteLine("Directory backbone is deployed.\n");
                    Console.WriteLine(String.Format("To directly access the API and its supporting documentation visit:\nhttp://{0}.azurewebsites.net/\n", WebApiName));
                    Console.WriteLine(String.Format("The Web Management Portal is now available at:\nhttp://{0}.azurewebsites.net/", WebInterfaceName));
                    Console.WriteLine("The default username is \"adminUser\" and the password is \"GoBeavs247\"");
                    Console.WriteLine("Please login to the Web Management Portal and change the default password as");
                    Console.WriteLine("soon as possible.");
                    Console.WriteLine("A Windows Phone application is also available as a user-friendly interface");
                    Console.WriteLine("for the API. Please refer to the system documentation for deployment");
                    Console.WriteLine("instructions on the Windows Phone application");
                }
                catch (System.IO.IOException ioex)
                {
                    Console.WriteLine("IO Exception: " + ioex.ToString());
                }
                catch (Microsoft.WindowsAzure.Storage.StorageException stex)
                {
                    Console.WriteLine("Azure Storage Exception: " + stex.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.ToString());
                }
                finally
                {
                    Console.WriteLine("\nPress any key to continue");
                    Console.ReadLine();
                }
            }
        }

        private static void AddFirewallRule(SubscriptionCloudCredentials Credentials)
        {
            string WebManagementAppPrefix = StorageAccountName + "-management";
            SqlManagementClient client = new SqlManagementClient(Credentials);
            string AzureServices = "0.0.0.0";

            client.FirewallRules.Create(DBServerName, new FirewallRuleCreateParameters(WebManagementAppPrefix, AzureServices, AzureServices));            
        }

        private static void ConfigureApp(string Path)
        {
            string WebConfig = System.IO.Directory.GetCurrentDirectory() + Path + "Web.Config";
            string text = File.ReadAllText(WebConfig);
            text = text.Replace("{prefix}", WebApiName);
            text = text.Replace("{sitename}", DatabaseName);
            text = text.Replace("{servername}", DBServerName);
            text = text.Replace("{username}", DBUserName);
            text = text.Replace("{password}", DBPassword);
            text = text.Replace("{AccountName}", StorageAccountName);
            text = text.Replace("{AccessKey}", StorageAccessKey);
            File.WriteAllText(WebConfig, text);
        }

        private static void DeleteFirewallRule(SubscriptionCloudCredentials Credentials)
        {
            SqlManagementClient client = new SqlManagementClient(Credentials);
            client.FirewallRules.Delete(DBServerName, DBRuleName);            
        }

        private static void PopulateStorageAccount(SubscriptionCloudCredentials Credentials, string path, bool Verbose = false)
        {
            foreach (string SourceTable in System.IO.Directory.GetDirectories(path))
            {
                Console.WriteLine(String.Format("\nCreating {0} table in {1} storage account", SourceTable.Split('/').Last().Split('\\').Last(), StorageAccountName));
                // Create table in Azure 
                CloudStorageAccount StorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + StorageAccountName + ";AccountKey=" + StorageAccessKey + ";");
                CloudTableClient TableClient = StorageAccount.CreateCloudTableClient();
                CloudTable Table = TableClient.GetTableReference(SourceTable.Remove(0, path.Length));
                Table.CreateIfNotExists();
                Console.WriteLine("Table created");

                // Build AzCopy Command and Arguments 
                Console.WriteLine(String.Format("\nPopulating {0} table", SourceTable));
                StringBuilder Command = new StringBuilder(System.IO.Directory.GetCurrentDirectory() + @"\AzCopy\AzCopy.exe");
                Command.Append(" /Source:" + SourceTable);
                Command.Append(" /Dest:https://" + StorageAccountName + ".table.core.windows.net/" + Table.Name + "/");
                Command.Append(" /DestKey:" + StorageAccessKey);
                Command.Append(" /Manifest:\"" + Path.GetFileName(System.IO.Directory.GetFiles(SourceTable, "*.manifest").First()) + "\"");
                Command.Append(" /EntityOperation:InsertOrReplace /Y");

                // Run AzCopy 
                RunCommand(Command.ToString(), Verbose);

                Console.WriteLine(Table.Name + " population complete");
            }
        }

        private static void PopulateDatabase(SubscriptionCloudCredentials Credentials, string DatabaseName)
        {
            string ConnectionString = String.Format("Data Source=tcp:{0}.database.windows.net,1433;Initial Catalog={1};User Id={2}@{0};Password={3};", DBServerName, DatabaseName, DBUserName, DBPassword);
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Microsoft.SqlServer.Management.Smo.Server server = new Microsoft.SqlServer.Management.Smo.Server(new ServerConnection(Connection));
                server.ConnectionContext.ExecuteNonQuery(File.ReadAllText(System.IO.Directory.GetCurrentDirectory() + "\\CRR_db.sql"));
                Connection.Close();
            }
        }

        private static string CreateDatabase(SubscriptionCloudCredentials Credentials, string DatabaseName)
        {
            SqlManagementClient client = new SqlManagementClient(Credentials);
            int i = 0;
            bool ValidDatabaseName = false;
            string ModifiedDBName = DatabaseName;
            var ServerList = client.Servers.List().Where(x => (x.Location == LocationNames.WestUS) && (x.AdministratorUserName == DBUserName));            
            
            if (ServerList.Count() == 0)
                client.Servers.Create(new ServerCreateParameters() { Location = LocationNames.WestUS, AdministratorUserName = DBUserName, AdministratorPassword = DBPassword });
                
            var Server = client.Servers.List().Where(x => x.AdministratorUserName == DBUserName).First();

            DBServerName = Server.Name;
            string ExternalIPAddress = new System.Net.WebClient().DownloadString("http://bot.whatismyipaddress.com");

            // Temporary Firewall Rule for installation
            client.FirewallRules.Create(DBServerName, new FirewallRuleCreateParameters(DBRuleName, ExternalIPAddress, ExternalIPAddress));
            
            while (!ValidDatabaseName)
            {
                try
                {
                    var Database = client.Databases.Get(DBServerName, ModifiedDBName);

                    if (Database != null)
                        ModifiedDBName = DatabaseName + (i++).ToString();
                    else
                        throw new CloudException("");
                }
                catch (CloudException cex)
                {
                    if (cex.Response.StatusCode.ToString() == "NotFound")
                        ValidDatabaseName = true;
                    else
                        throw new CloudException("", cex);
                }
            }
                
            client.Databases.Create(Server.Name, new DatabaseCreateParameters() { Name = ModifiedDBName });
        
            return ModifiedDBName;
        }

        private static void DeployWebSite(SubscriptionCloudCredentials Credentials, string AppPath, string WebSiteName, bool Verbose = false)
        {
            var WebSiteClient = CloudContext.Clients.CreateWebSiteManagementClient(Credentials);
            var PublishProfile = WebSiteClient.WebSites.GetPublishProfile(WebSpaceName, WebSiteName).Where(x => x.PublishMethod == "FTP").First();

            using (FtpClient client = new FtpClient())
            {
                string Host = PublishProfile.PublishUrl.Substring(6).Split('/').First();
                string UploadPath = "/site/wwwroot";

                client.Host = Host;
                client.Credentials = new System.Net.NetworkCredential(PublishProfile.UserName, PublishProfile.UserPassword);
                client.DataConnectionType = FtpDataConnectionType.PASV;
                client.ConnectTimeout = 60000;
                client.DataConnectionConnectTimeout = 60000;
                client.SocketKeepAlive = true;
                client.Connect();

                client.DeleteFile(UploadPath + "/hostingstart.html");

                RecursiveDirectoryUpload(client, System.IO.Directory.GetCurrentDirectory() + AppPath, UploadPath, Verbose);
                
                client.Disconnect();
            }

        }

        private static void RecursiveDirectoryUpload(FtpClient ftpClient, string dirPath, string uploadPath, bool Verbose = false)
        {
            string[] files = Directory.GetFiles(dirPath, "*.*");
            string[] subDirs = Directory.GetDirectories(dirPath);

            //foreach (string file in files)
            Parallel.ForEach(files, file =>
            {
                bool FileUploaded = false;
                int TimeOutCount = 0;
                string FileName = file.Split('\\').Last().Split('/').Last();
                while (!FileUploaded)
                {
                    try
                    {
                        using (var fileStream = File.OpenRead(file))
                        {
                            using (var ftpStream = ftpClient.OpenWrite(string.Format("{0}/{1}", uploadPath, Path.GetFileName(file))))
                            {
                                fileStream.CopyTo(ftpStream);
                                FileUploaded = true;

                            }
                        }

                    }
                    catch (TimeoutException tex)
                    {
                        if (TimeOutCount >= 3)
                            throw new TimeoutException("", tex);
                        FileUploaded = false;
                        Console.Write(Verbose ? String.Format("{1} upload timed out\nReconnecting ({0} of 3)...", (TimeOutCount + 1).ToString(), FileName) : ".");
                    }
                }

                Console.Write(Verbose ? FileName + "...Uploaded\n" : ".");
            });

            foreach (string subDir in subDirs)
            {
                ftpClient.CreateDirectory(uploadPath + "/" + Path.GetFileName(subDir));
                RecursiveDirectoryUpload(ftpClient, subDir, uploadPath + "/" + Path.GetFileName(subDir), Verbose);
            }
        }

        private static string GetStorageAccessKey(SubscriptionCloudCredentials Credentials)
        {
            var storageClient = CloudContext.Clients.CreateStorageManagementClient(Credentials);
            return storageClient.StorageAccounts.GetKeys(StorageAccountName).SecondaryKey.ToString();
        }

        static SubscriptionCloudCredentials getCredentials(string subscriptionId)
        {
            return new CertificateCloudCredentials(subscriptionId, new X509Certificate2(System.IO.Directory.GetCurrentDirectory() + CertificatePath));
        }

        /* Runs the Windows command: cmd.exe and arguments */
        private static void RunCommand(string Arguments, bool Verbose = false)
        {
            try
            {
                ProcessStartInfo CommandInfo = new ProcessStartInfo("cmd.exe", "/C \"" + Arguments + "\"");
                CommandInfo.WindowStyle = ProcessWindowStyle.Hidden;
                CommandInfo.UseShellExecute = false;
                CommandInfo.RedirectStandardOutput = !Verbose;

                Process Executable = new Process();
                Executable.StartInfo = CommandInfo;
                Executable.Start();
                Executable.WaitForExit();
            }
            catch (Exception ex)
            {
                throw new Exception("cmd.exe Exception: " + ex.ToString());
            }
        }

        private static Task<Microsoft.WindowsAzure.OperationStatusResponse> CreateStorageAccountName(SubscriptionCloudCredentials Credentials)
        {
            int i = 0;
            var storageClient = CloudContext.Clients.CreateStorageManagementClient(Credentials);
            string storageAccountName = "crr";
            bool validStorageName = false;
            
            while (!validStorageName)
            {
                try
                {
                    var availabilityResponse = storageClient.StorageAccounts.CheckNameAvailability(storageAccountName);
                    
                    if(!(validStorageName = availabilityResponse.IsAvailable))
                        storageAccountName = "crr" + (i++).ToString();                    
                }
                catch
                {
                    validStorageName = false;
                }
            }
            StorageAccountName = storageAccountName;
            return storageClient.StorageAccounts.CreateAsync(new StorageAccountCreateParameters()
            {
                Location = LocationNames.WestUS,
                Name = storageAccountName,
                Description = "storage account for corvallis reuse and recycle directory",
                AccountType = "Standard_GRS"
            });
        }

        public static string CreateWebApp(SubscriptionCloudCredentials Credentials, string SiteName)
        {
            var WebSiteClient = CloudContext.Clients.CreateWebSiteManagementClient(Credentials);
            var WebSpace = WebSiteClient.WebSpaces.List().First(x => x.GeoRegion == "West US");
            WebSpaceName = WebSpace.Name;
            var WebHostingPlan = WebSiteClient.WebHostingPlans.List(WebSpaceName).First();
            string WebHostingPlanName = "", ModifiedSiteName = "";
            int i = 0;
            bool ValidWebSiteName = false;

            if (WebHostingPlan == null)
            {
                WebHostingPlanName = "Default-West-US";
                WebSiteClient.WebHostingPlans.Create(WebSpaceName, new WebHostingPlanCreateParameters() { Name = WebHostingPlanName });
            }
            else
                WebHostingPlanName = WebHostingPlan.Name;

            ModifiedSiteName = SiteName;

            while (!ValidWebSiteName)
            {
                try
                {
                    var WebSite = WebSiteClient.WebSites.Get(WebSpaceName, ModifiedSiteName, null);

                    if (WebSite != null)
                        ModifiedSiteName = SiteName + (i++).ToString();
                    else
                        throw new CloudException("") { ErrorCode = "NotFound" };
                }
                catch (CloudException cex)
                {
                    if (cex.ErrorCode == "NotFound")
                        ValidWebSiteName = true;
                    else
                        throw new Exception("", cex);
                }
            }
            
            WebSiteClient.WebSites.Create(WebSpaceNames.WestUSWebSpace, new WebSiteCreateParameters() { Name = ModifiedSiteName, ServerFarm = WebHostingPlanName });

            return ModifiedSiteName;
        }
    }
}
