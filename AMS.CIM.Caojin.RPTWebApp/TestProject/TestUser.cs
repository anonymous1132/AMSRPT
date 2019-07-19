using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;

namespace TestProject
{
    public class TestUser
    {
        [DllImport("advapi32.DLL", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);
        [DllImport("kernel32.dll")]
        public static extern int WinExec(string exeName, int operType);

        [StructLayout(LayoutKind.Sequential)]
        public struct SHELLEXECUTEINFO
        {
            public int cbSize;
            public uint fMask;
            public IntPtr hwnd;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpVerb;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpFile;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpParameters;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpDirectory;
            public int nShow;
            public IntPtr hInstApp;
            public IntPtr lpIDList;
            [MarshalAs(UnmanagedType.LPTStr)]
            public string lpClass;
            public IntPtr hkeyClass;
            public uint dwHotKey;
            public IntPtr hIcon;
            public IntPtr hProcess;
        }

        public enum ShowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_FORCEMINIMIZE = 11,
            SW_MAX = 11
        }

        [Flags]
        public enum ShellExecuteMaskFlags : uint
        {
            SEE_MASK_DEFAULT = 0x00000000,
            SEE_MASK_CLASSNAME = 0x00000001,
            SEE_MASK_CLASSKEY = 0x00000003,
            SEE_MASK_IDLIST = 0x00000004,
            SEE_MASK_INVOKEIDLIST = 0x0000000c,   // Note SEE_MASK_INVOKEIDLIST(0xC) implies SEE_MASK_IDLIST(0x04) 
            SEE_MASK_HOTKEY = 0x00000020,
            SEE_MASK_NOCLOSEPROCESS = 0x00000040,
            SEE_MASK_CONNECTNETDRV = 0x00000080,
            SEE_MASK_NOASYNC = 0x00000100,
            SEE_MASK_FLAG_DDEWAIT = SEE_MASK_NOASYNC,
            SEE_MASK_DOENVSUBST = 0x00000200,
            SEE_MASK_FLAG_NO_UI = 0x00000400,
            SEE_MASK_UNICODE = 0x00004000,
            SEE_MASK_NO_CONSOLE = 0x00008000,
            SEE_MASK_ASYNCOK = 0x00100000,
            SEE_MASK_HMONITOR = 0x00200000,
            SEE_MASK_NOZONECHECKS = 0x00800000,
            SEE_MASK_NOQUERYCLASSSTORE = 0x01000000,
            SEE_MASK_WAITFORINPUTIDLE = 0x02000000,
            SEE_MASK_FLAG_LOG_USAGE = 0x04000000,
        }

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        static extern bool ShellExecuteEx(ref SHELLEXECUTEINFO lpExecInfo);
        private const int SW_SHOW = 5;
        private const uint SEE_MASK_INVOKEIDLIST = 12;
        public static void Test()
        {
            string programName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            string dirPath = System.IO.Path.GetDirectoryName(programName);
            string curPath = System.IO.Path.Combine(dirPath, "CurUser");
            string adminPath = System.IO.Path.Combine(dirPath, "Admin");
            var curFiles = System.IO.Directory.GetFiles(curPath);
            var adminFiles = System.IO.Directory.GetFiles(adminPath);
            var dirs = Directory.GetDirectories(dirPath);
            //var adminFiles = System.IO.Directory.GetFiles(adminPath);
            Console.Write("管理员用户名：");
            string user = Console.ReadLine();
            string pass = GetPassword();
            //IntPtr admin_token = default(IntPtr);
            //WindowsIdentity wid_admin = null;
            //WindowsImpersonationContext wic = null;
            //while (!LogonUser(user, "amsc", pass, 2, 0, ref admin_token))
            //{
            //    Console.WriteLine("验证失败，请重新输入");
            //    Console.Write("管理员用户名：");
            //    user = Console.ReadLine();
            //    pass = GetPassword();
            //}
            IdentityAnalogue ia = new IdentityAnalogue();
            while (!ia.ImpersonateValidUser(user, "amsc", pass))
            {
                Console.Write("管理员用户名：");
                user = Console.ReadLine();
                pass = GetPassword();
            }

  
            //using (wid_admin = new WindowsIdentity(admin_token))
            //{
            //    using (wic = wid_admin.Impersonate())
            //    {
            //        Console.WriteLine("模拟用户：{0}",Environment.UserName); //测试结果是输入的用户
            //                                                 //RunTest(user,pass);                               //测试结果是登录用户
            //        //WindowsIdentity admUser = WindowsIdentity.GetCurrent();
            //        //SecurityIdentifier admSid = admUser.User;
            //        //NTAccount admNtacc = (NTAccount)admSid.Translate(typeof(NTAccount));
            //        //Console.WriteLine(admNtacc.Value);
            //        foreach (var f in adminFiles)
            //        {
            //            RunExcuteableFile(f);
            //        }
            //    }
            //}
            Console.WriteLine("模拟用户：{0}", Environment.UserName);
            //string testPath = @"\\10.8.0.252\信息运维部\个人目录";

            foreach (var d in dirs)
            {
                Console.WriteLine(d);
            }
            foreach (var f in adminFiles)
            {
               // Runas(f);
            }
            Runas("cmd.exe");
            ia.UndoImpersonation();
            // TestProcess.CreateProcess(admin_token, @"D:\Program Files (x86)\Nmap\zenmap.exe");

            Console.WriteLine("执行用户：{0}",Environment.UserName);
            foreach (var f in curFiles)
            {
               RunExcuteableFile(f);
            }
            Console.WriteLine("所有程序执行完毕后按Enter键退出");
            Console.ReadLine();
        }

        public static void RunExcuteableFile(string filePath)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            
            if (!fi.Exists) return;
            if (fi.Extension.ToLower() == ".exe" || fi.Extension.ToLower() == ".bat")
            {
                //var p = new Process
                //{
                //    StartInfo = new ProcessStartInfo()
                //    {
                //        FileName = filePath,
                //        UseShellExecute = false,
                //        RedirectStandardInput = true,
                //        RedirectStandardOutput = true,
                //        RedirectStandardError = true,
                //        CreateNoWindow = true,
                //        Verb = "runas"
                //    }
                //};
                //p.Start();
                //p.WaitForExit();
                //p.Close();
                Runas(filePath);
            }
            if (fi.Extension.ToLower() == ".reg")
            {
                filePath = @"""" + filePath + @"""";
                Process.Start("regedit", filePath);
            }
        }



        public static void RunExcuteableFile(string filePath, string user, string pass)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(filePath);
            if (!fi.Exists) return;
            if (fi.Extension.ToLower() == ".exe" || fi.Extension.ToLower() == ".bat")
            {
                var p = new Process
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        FileName = filePath,
                        UseShellExecute = false,
                        RedirectStandardInput = true,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        Password = new System.Security.SecureString(),
                        UserName = "liheng",
                        Domain = "amsc",
                        Verb = "runas"
                    }
                };
                foreach (char c in "lh@2019#".ToCharArray())
                {
                    p.StartInfo.Password.AppendChar(c);
                }
                p.Start();
                p.WaitForExit();
                p.Close();
            }
            if (fi.Extension.ToLower() == ".reg")
            {
                filePath = @"""" + filePath + @"""";
                Process.Start("regedit", filePath);
            }
        }
        public static string GetPassword()
        {
            string input = "";
            Console.Write("管理员口令：");
            while (true)
            {
                //存储用户输入的按键，并且在输入的位置不显示字符
                ConsoleKeyInfo ck = Console.ReadKey(true);

                //判断用户是否按下的Enter键
                if (ck.Key != ConsoleKey.Enter)
                {
                    if (ck.Key != ConsoleKey.Backspace)
                    {
                        //将用户输入的字符存入字符串中
                        input += ck.KeyChar.ToString();
                        //将用户输入的字符替换为*
                        Console.Write("*");
                    }
                    else
                    {
                        //删除错误的字符
                        if (input != "")
                        {
                            Console.Write("\b \b");
                        }
                    }
                }
                else
                {
                    Console.WriteLine();

                    break;
                }
            }
            return input;
        }

        public static void RunTest(string user, string pass)
        {
            using (var process = new Process())
            {
                process.StartInfo.FileName = "c:\\windows\\system32\\cmd.exe";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = false;
                process.StartInfo.UserName = user;
                process.StartInfo.Domain = "amsc";
                // process.StartInfo.PasswordInClearText = pass;
                process.StartInfo.Password = new System.Security.SecureString();
                foreach (char c in pass.ToCharArray())
                {
                    process.StartInfo.Password.AppendChar(c);
                }
                process.Start();
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("echo %username%" + "&exit");

                //获取cmd窗口的输出信息  
                string output = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();
                Console.WriteLine(output);
            }
        }

        public static void Runas(string Filename)
        {
            SHELLEXECUTEINFO info = new SHELLEXECUTEINFO();
            info.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(info);
            info.lpVerb = "runas";
            info.lpFile = Filename;
            info.nShow = 5;
            info.fMask = SEE_MASK_INVOKEIDLIST;
            ShellExecuteEx(ref info);
        }
    }
}
