using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;


public class TestProcess
{

    public static IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

    #region P/Invoke WTS APIs
    private enum WTS_CONNECTSTATE_CLASS
    {
        WTSActive,
        WTSConnected,
        WTSConnectQuery,
        WTSShadow,
        WTSDisconnected,
        WTSIdle,
        WTSListen,
        WTSReset,
        WTSDown,
        WTSInit
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    private struct WTS_SESSION_INFO
    {
        public UInt32 SessionID;
        public string pWinStationName;
        public WTS_CONNECTSTATE_CLASS State;
    }

    [DllImport("WTSAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
    static extern bool WTSEnumerateSessions(
        IntPtr hServer,
        [MarshalAs(UnmanagedType.U4)] UInt32 Reserved,
        [MarshalAs(UnmanagedType.U4)] UInt32 Version,
        ref IntPtr ppSessionInfo,
        [MarshalAs(UnmanagedType.U4)] ref UInt32 pSessionInfoCount
        );

    [DllImport("WTSAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
    static extern void WTSFreeMemory(IntPtr pMemory);

    [DllImport("WTSAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
    static extern bool WTSQueryUserToken(UInt32 sessionId, out IntPtr Token);
    #endregion

    #region P/Invoke CreateProcessAsUser
    /// <summary> 
    /// Struct, Enum and P/Invoke Declarations for CreateProcessAsUser. 
    /// </summary> 
    ///  

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct STARTUPINFO
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

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    struct PROCESS_INFORMATION
    {
        public IntPtr hProcess;
        public IntPtr hThread;
        public int dwProcessId;
        public int dwThreadId;
    }

    /// <summary>
    /// 以当前登录的windows用户(角色权限)运行指定程序进程
    /// </summary>
    /// <param name="hToken"></param>
    /// <param name="lpApplicationName">指定程序(全路径)</param>
    /// <param name="lpCommandLine">参数</param>
    /// <param name="lpProcessAttributes">进程属性</param>
    /// <param name="lpThreadAttributes">线程属性</param>
    /// <param name="bInheritHandles"></param>
    /// <param name="dwCreationFlags"></param>
    /// <param name="lpEnvironment"></param>
    /// <param name="lpCurrentDirectory"></param>
    /// <param name="lpStartupInfo">程序启动属性</param>
    /// <param name="lpProcessInformation">最后返回的进程信息</param>
    /// <returns>是否调用成功</returns>
    [DllImport("ADVAPI32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
    static extern bool CreateProcessAsUser(IntPtr hToken, string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes,
                                                  bool bInheritHandles, uint dwCreationFlags, string lpEnvironment, string lpCurrentDirectory,
                                                  ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

    [DllImport("KERNEL32.DLL", SetLastError = true, CharSet = CharSet.Auto)]
    static extern bool CloseHandle(IntPtr hHandle);
    #endregion

    /// <summary>
    /// 以当前登录系统的用户角色权限启动指定的进程
    /// </summary>
    /// <param name="ChildProcName">指定的进程(全路径)</param>
    public static void CreateProcess(string ChildProcName)
    {
        IntPtr ppSessionInfo = IntPtr.Zero;
        UInt32 SessionCount = 0;
        if (WTSEnumerateSessions(
                                (IntPtr)WTS_CURRENT_SERVER_HANDLE,  // Current RD Session Host Server handle would be zero. 
                                0,  // This reserved parameter must be zero. 
                                1,  // The version of the enumeration request must be 1. 
                                ref ppSessionInfo, // This would point to an array of session info. 
                                ref SessionCount  // This would indicate the length of the above array.
                                ))
        {
            for (int nCount = 0; nCount < SessionCount; nCount++)
            {
                WTS_SESSION_INFO tSessionInfo = (WTS_SESSION_INFO)Marshal.PtrToStructure(ppSessionInfo + nCount * Marshal.SizeOf(typeof(WTS_SESSION_INFO)), typeof(WTS_SESSION_INFO));
                if (WTS_CONNECTSTATE_CLASS.WTSActive == tSessionInfo.State)
                {
                    IntPtr hToken = IntPtr.Zero;
                    if (WTSQueryUserToken(tSessionInfo.SessionID, out hToken))
                    {
                        PROCESS_INFORMATION tProcessInfo;
                        STARTUPINFO tStartUpInfo = new STARTUPINFO();
                        tStartUpInfo.cb = Marshal.SizeOf(typeof(STARTUPINFO));
                        bool ChildProcStarted = CreateProcessAsUser(
                                                                    hToken,             // Token of the logged-on user. 
                                                                    ChildProcName,      // Name of the process to be started. 
                                                                    null,               // Any command line arguments to be passed. 
                                                                    IntPtr.Zero,        // Default Process' attributes. 
                                                                    IntPtr.Zero,        // Default Thread's attributes. 
                                                                    false,              // Does NOT inherit parent's handles. 
                                                                    0,                  // No any specific creation flag. 
                                                                    null,               // Default environment path. 
                                                                    null,               // Default current directory. 
                                                                    ref tStartUpInfo,   // Process Startup Info.  
                                                                    out tProcessInfo    // Process information to be returned. 
                                                 );
                        if (ChildProcStarted)
                        {
                            CloseHandle(tProcessInfo.hThread);
                            CloseHandle(tProcessInfo.hProcess);
                        }
                        else
                        {
                            Console.WriteLine("CreateProcessAsUser失败");
                        }
                        CloseHandle(hToken);
                        break;
                    }
                }
            }
            WTSFreeMemory(ppSessionInfo);
        }
    }

    public static void CreateProcess(IntPtr admin_token,string procName)
    {
        PROCESS_INFORMATION tProcessInfo;
        STARTUPINFO tStartUpInfo = new STARTUPINFO();
        tStartUpInfo.cb = Marshal.SizeOf(typeof(STARTUPINFO));
        bool ChildProcStarted = CreateProcessAsUser(
                                                    admin_token,             // Token of the logged-on user. 
                                                    procName,      // Name of the process to be started. 
                                                    null,               // Any command line arguments to be passed. 
                                                    IntPtr.Zero,        // Default Process' attributes. 
                                                    IntPtr.Zero,        // Default Thread's attributes. 
                                                    false,              // Does NOT inherit parent's handles. 
                                                    0,                  // No any specific creation flag. 
                                                    null,               // Default environment path. 
                                                    null,               // Default current directory. 
                                                    ref tStartUpInfo,   // Process Startup Info.  
                                                    out tProcessInfo    // Process information to be returned. 
                                 );
        if (ChildProcStarted)
        {
            CloseHandle(tProcessInfo.hThread);
            CloseHandle(tProcessInfo.hProcess);
        }
        else
        {
            Console.WriteLine("CreateProcessAsUser失败");
        }
        CloseHandle(admin_token);
    }

    }