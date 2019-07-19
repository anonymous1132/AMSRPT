using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;

namespace TestProject
{
    public class IdentityAnalogue
    {

        public const int LOGON32_LOGON_INTERACTIVE = 2;
        /**//// <summary>
            /// 
            /// </summary>
        public const int LOGON32_PROVIDER_DEFAULT = 0;

        /**//// <summary>
            /// 
            /// </summary>
        WindowsImpersonationContext impersonationContext;

        //win32api引用
        /**//// <summary>
            /// 
            /// </summary>
            /// <param name="lpszUserName"></param>
            /// <param name="lpszDomain"></param>
            /// <param name="lpszPassword"></param>
            /// <param name="dwLogonType"></param>
            /// <param name="dwLogonProvider"></param>
            /// <param name="phToken"></param>
            /// <returns></returns>
        [DllImport("advapi32.dll")]
        public static extern int LogonUserA(string lpszUserName,
         string lpszDomain,
         string lpszPassword,
         int dwLogonType,
         int dwLogonProvider,
         ref IntPtr phToken);
        /**//// <summary>
            /// 
            /// </summary>
            /// <param name="hToken"></param>
            /// <param name="impersonationLevel"></param>
            /// <param name="hNewToken"></param>
            /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DuplicateToken(IntPtr hToken,
         int impersonationLevel,
         ref IntPtr hNewToken);
        /**//// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool RevertToSelf();
        /**//// <summary>
            /// 
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool CloseHandle(IntPtr handle);
        /**//// <summary>
            /// 
            /// </summary>
        public IdentityAnalogue()
        {
        }


        //模拟指定的用户身份
        /**//// <summary>
            /// 
            /// </summary>
            /// <param name="userName"></param>
            /// <param name="domain"></param>
            /// <param name="password"></param>
            /// <returns></returns>
        public bool ImpersonateValidUser(string userName, string domain, string password)
        {
            WindowsIdentity tempWindowsIdentity;
            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;
            if (RevertToSelf())
            {
                if (LogonUserA(userName, domain, password, 2, 0, ref token) != 0)
                {
                    if (DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                    {
                        tempWindowsIdentity = new WindowsIdentity(tokenDuplicate);
                        impersonationContext = tempWindowsIdentity.Impersonate();
                        if (impersonationContext != null)
                        {
                            CloseHandle(token);
                            CloseHandle(tokenDuplicate);
                            return true;
                        }
                    }
                }
            }
            if (token != IntPtr.Zero)
                CloseHandle(token);
            if (tokenDuplicate != IntPtr.Zero)
                CloseHandle(tokenDuplicate);
            return false;
        }

        //取消模拟
        /**//// <summary>
            /// 
            /// </summary>
        public void UndoImpersonation()
        {
            impersonationContext.Undo();
        }
    }
}
    


