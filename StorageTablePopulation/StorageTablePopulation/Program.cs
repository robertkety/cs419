using CommandLine;
using CommandLine.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace StorageTablePopulation
{
    class Program
    {
        // Thanks to http://commandline.codeplex.com
        class Options
        {
            [Option('n', "name", Required = true,
              HelpText = "Name of Azure Storage Account")]
            public string AccountName { get; set; }

            [Option('k', "key", Required = true,
              HelpText = "Access Key for Azure Storage Account")]
            public string AccessKey { get; set; }

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
            var Options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(Args, Options))
            {
                string path = "./StorageTables/";
                try
                {
                    foreach (string SourceTable in System.IO.Directory.GetDirectories(path))
                    {
                        /* Create table in Azure */
                        CloudStorageAccount StorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=" + Options.AccountName + ";AccountKey=" + Options.AccessKey + ";");
                        CloudTableClient TableClient = StorageAccount.CreateCloudTableClient();
                        CloudTable Table = TableClient.GetTableReference(SourceTable.Remove(0, path.Length));
                        Table.CreateIfNotExists();

                        /* Build AzCopy Command and Arguments */
                        StringBuilder Command = new StringBuilder(System.IO.Directory.GetCurrentDirectory() + @"\AzCopy\AzCopy.exe");
                        Command.Append(" /Source:" + SourceTable);
                        Command.Append(" /Dest:https://" + Options.AccountName + ".table.core.windows.net/" + Table.Name + "/");
                        Command.Append(" /DestKey:" + Options.AccessKey);
                        Command.Append(" /Manifest:\"" + Path.GetFileName(System.IO.Directory.GetFiles(SourceTable, "*.manifest").First()) + "\"");
                        Command.Append(" /EntityOperation:InsertOrReplace /Y");

                        /* Run AzCopy */
                        RunCommand(Command.ToString(), Options.Verbose);
                        
                        Console.WriteLine(Table.Name + " upload complete\n");
                    }

                    Console.WriteLine("\nStorage Table Population Complete");
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
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }

                
            }
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
    }
}
