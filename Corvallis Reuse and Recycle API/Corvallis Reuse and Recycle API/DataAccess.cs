using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Data;
using System.Windows.Forms;
//Add MySql Library
using MySql.Data.Entity;
using MySql.Data.Types;

// SSH
using Renci.SshNet;
using Renci.SshNet.Common;
using System.Net;

namespace Corvallis_Reuse_and_Recycle_API
{
    public class DataAccess
    {
        public static List<string> GetCategories()
        {
            MySqlConnection conn = new MySqlConnection(ConfigurationManager.ConnectionStrings["MySQLDBConnection"].ConnectionString);
            List<string> output = new List<string>();
            try
            {
                using (var client = new SshClient("flip1.engr.oregonstate.edu", "ketyr", "!Cop5665"))
                {

                    var port = new ForwardedPortLocal(3306, IPAddress.Loopback.ToString(), 3306);
                    client.AddForwardedPort(port);
                    client.Connect();

                    var connection = conn;   //new MySqlConnection("server=localhost;database=[database];uid=ubuntu;");
                    MySqlCommand command = connection.CreateCommand();
                    MySqlDataReader Reader;
                    command.CommandText = "select * from Category";
                    connection.Open();
                    Reader = command.ExecuteReader();
                    while (Reader.Read())
                    {
                        string thisrow = "";
                        for (int i = 0; i < Reader.FieldCount; i++)
                            thisrow += Reader.GetValue(i).ToString() + ",";
                        output.Add(thisrow);
                    }
                    connection.Close();
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                output.Add(ex.ToString());
            }/*
            
            PasswordConnectionInfo connectionInfo = new PasswordConnectionInfo("flip1.engr.oregonstate.edu", "ketyr", "!Cop5665");
            connectionInfo.Timeout = TimeSpan.FromSeconds(30);

            using (var client = new SshClient(connectionInfo))
            {
                try
                {
                    output.Add("Trying SSH connection...");
                    client.Connect();
                    if (client.IsConnected)
                    {
                        Debug.WriteLine("SSH connection is active: {0}", client.ConnectionInfo.ToString());
                    }
                    else
                    {
                        output.Add(String.Format("SSH connection has failed: {0}", client.ConnectionInfo.ToString()));
                    }

                    output.Add(String.Format("\r\nTrying port forwarding..."));
                    var portFwld = new ForwardedPortLocal(Convert.ToUInt32(4479), IPAddress.Loopback.ToString(), Convert.ToUInt32(3306));
                    client.AddForwardedPort(portFwld);
                    portFwld.Start();
                    if (portFwld.IsStarted)
                    {
                        output.Add(String.Format("Port forwarded: {0}", portFwld.ToString()));
                    }
                    else
                    {
                       output.Add(String.Format("Port forwarding has failed."));
                    }
                    
                }
                catch (SshException e)
                {
                    output.Add(String.Format("SSH client connection error: {0}", e.Message));
                }
                catch (System.Net.Sockets.SocketException e)
                {
                    output.Add(String.Format("Socket connection error: {0}", e.Message));
                }

                try
                {
                    //Debug.WriteLine("\r\nTrying database connection...");
                    DBConnect dbConnect = new DBConnect("mysql.eecs.oregonstate.edu", "cs419-g2", "cs419-g2", "GtjpTvRTeqUE429E", "4479");

                    var ct = dbConnect.Count("Category");
                    output.Add(ct.ToString());

                    //conn.Open();
                    //Debug.WriteLine("MySQL version : {0}", conn.ServerVersion);
                    //output.Add(conn.ServerVersion);

                }
                catch (MySqlException ex)
                {
                    Debug.WriteLine(ex);
                    //Console.WriteLine("Error: {0}", ex.ToString());
                    output.Add(ex.ToString());
                }
                catch (Exception ex)
                {
                    output.Add(ex.ToString());
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
            }
            */
            
            return output;
        }

        
		public static void Main (string[] args)
        {
            
        }
    }

    // MySQL DB class
    class DBConnect
    {
        private MySqlConnection connection;

        private string server;
        public string Server
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        private string database;
        public string Database
        {
            get
            {
                return this.database;
            }
            set
            {
                this.database = value;
            }
        }

        private string uid;
        public string Uid
        {
            get
            {
                return this.server;
            }
            set
            {
                this.server = value;
            }
        }

        private string password;
        public string Password
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        private string port;
        public string Port
        {
            get
            {
                return this.port;
            }
            set
            {
                this.port = value;
            }
        }

        //Constructor
        public DBConnect(string server, string database, string uid, string password, string port = "3306")
        {
            this.server = server;

            this.database = database;
            this.uid = uid;
            this.password = password;
            this.port = port;

            Initialize();
        }

        //Initialize values
        private void Initialize()
        {
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }


        //open connection to database
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                Debug.WriteLine("MySQL connected.");
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                    Debug.WriteLine("Cannot connect to server.  Contact administrator");
                    break;

                    case 1045:
                    Debug.WriteLine("Invalid username/password, please try again");
                    break;

                    default:
                    Debug.WriteLine("Unhandled exception: {0}.", ex.Message);
                    break;

                }
                return false;
            }
        }

        //Close connection
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        //Insert statement
        public void Insert()
        {
            string query = "INSERT INTO tableinfo (name, age) VALUES('John Smith', '33')";

            //open connection
            if (this.OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //Execute command
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Update statement
        public void Update(string tableName, List<KeyValuePair<string, string>> setArgs, List<KeyValuePair<string, string>> whereArgs)
        {
            string query = "UPDATE tableinfo SET name='Joe', age='22' WHERE name='John Smith'";

            //Open connection
            if (this.OpenConnection() == true)
            {
                //create mysql command
                MySqlCommand cmd = new MySqlCommand();
                //Assign the query using CommandText
                cmd.CommandText = query;
                //Assign the connection using Connection
                cmd.Connection = connection;

                //Execute query
                cmd.ExecuteNonQuery();

                //close connection
                this.CloseConnection();
            }
        }

        //Delete statement
        public void Delete(string tableName, List<KeyValuePair<string, string>> whereArgs)
        {
            string query = "DELETE FROM tableinfo WHERE name='John Smith'";

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }
        }

        //Select statement
        public List<string> Select(string queryString)
        {
            string query = queryString;

            //Create a list to store the result
            List<string> list = new List<string>();

            //Open connection
            if (this.OpenConnection() == true)
            {
                //Create Command
                MySqlCommand cmd = new MySqlCommand(query, connection);
                //Create a data reader and Execute the command
                MySqlDataReader dataReader = cmd.ExecuteReader();

                //Read the data and store them in the list
                int fieldCOunt = dataReader.FieldCount;
                while (dataReader.Read())
                {
                    for (int i = 0; i < fieldCOunt; i++) {
                        list.Add(dataReader.GetValue(i).ToString());
                    }
                }

                //close Data Reader
                dataReader.Close();

                //close Connection
                this.CloseConnection();

                //return list to be displayed
                return list;
            }

            return list;

        }

        //Count statement
        public int Count(string tableName)
        {
            string query = "SELECT Count(*) FROM " + tableName;
            int Count = -1;

            //Open Connection
            if (this.OpenConnection() == true)
            {
                //Create Mysql Command
                MySqlCommand cmd = new MySqlCommand(query, connection);

                //ExecuteScalar will return one value
                Count = int.Parse(cmd.ExecuteScalar()+"");

                //close Connection
                this.CloseConnection();

                return Count;
            }

            return Count;

        }

        //Backup
        public void Backup()
        {
            try
            {
                DateTime Time = DateTime.Now;
                int year = Time.Year;
                int month = Time.Month;
                int day = Time.Day;
                int hour = Time.Hour;
                int minute = Time.Minute;
                int second = Time.Second;
                int millisecond = Time.Millisecond;

                //Save file to C:\ with the current date as a filename
                string path;
                path = "C:\\" + year + "-" + month + "-" + day + "-" + hour + "-" + minute + "-" + second + "-" + millisecond + ".sql";
                StreamWriter file = new StreamWriter(path);


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysqldump";
                psi.RedirectStandardInput = false;
                psi.RedirectStandardOutput = true;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;

                Process process = Process.Start(psi);

                string output;
                output = process.StandardOutput.ReadToEnd();
                file.WriteLine(output);
                process.WaitForExit();
                file.Close();
                process.Close();
            }
            catch (IOException e)
            {
                Debug.WriteLine("Error {0}, unable to backup!", e.Message);
            }
        }

        //Restore
        public void Restore()
        {
            try
            {
                //Read file from C:\
                string path;
                path = "C:\\MySqlBackup.sql";
                StreamReader file = new StreamReader(path);
                string input = file.ReadToEnd();
                file.Close();


                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = "mysql";
                psi.RedirectStandardInput = true;
                psi.RedirectStandardOutput = false;
                psi.Arguments = string.Format(@"-u{0} -p{1} -h{2} {3}", uid, password, server, database);
                psi.UseShellExecute = false;


                Process process = Process.Start(psi);
                process.StandardInput.WriteLine(input);
                process.StandardInput.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (IOException e)
            {
                Debug.WriteLine("Error {0}, unable to Restore!", e.Message);
            }
        }
    }
}
