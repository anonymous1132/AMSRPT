using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using Caojin.Common;
using AMS.CIM.Caojin.RPTDataUpdateService.Runner;


namespace AMS.CIM.Caojin.RPTDataUpdateService
{
    partial class RptDataRunnerService : ServiceBase
    {
        public RptDataRunnerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            LogHelper.InfoLog("RPTDataRunnerService启动成功");
            //10分钟运行一次
            System.Timers.Timer t = new System.Timers.Timer
            {
                Interval = 10*60*1000
            };
            t.Elapsed += new System.Timers.ElapsedEventHandler(ChkSrv);//到达时间的时候执行事件；   
            t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；   
            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；   
            // AMSDCMDataTranslator.Program.Main(null);
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            LogHelper.InfoLog("RPTDataRunnerService被关闭");
        }

        /// <summary>  
        /// 定时检查，并执行方法  
        /// </summary>  
        /// <param name="source"></param>  
        /// <param name="e"></param>  
        public void ChkSrv(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                System.Timers.Timer tt = (System.Timers.Timer)source;
                //可防止重复执行程序
                tt.Enabled = false;

                //EQP_UPm_018Runner不可在8:00至8:10之间执行,以确保隔日数据全部更新
                if (DateTime.Now.Hour != 8 || DateTime.Now.Minute > 10)
                {
                    EQP_UPm_018Runner.Run();
                }
                
                Lin_RealTime_025Runner.Run();
                if(DateTime.Now.Hour==0&&DateTime.Now.Minute<20)
                {
                    CycleTimeRunner.RunFlow();
                    CycleTimeRunner.RunCT();
                }
                tt.Enabled = true;
            }
            catch (Exception err)
            {
                LogHelper.ErrorLog(err);
            }

        }

        ///在指定时间过后执行指定的表达式  
        ///  
        ///事件之间经过的时间（以毫秒为单位）  
        ///要执行的表达式  
        public static void SetTimeout(double interval, Action action)
        {
            System.Timers.Timer timer = new System.Timers.Timer(interval);
            timer.Elapsed += delegate (object sender, System.Timers.ElapsedEventArgs e)
            {
                timer.Enabled = false;
                action();
            };
            timer.Enabled = true;
        }
    }
}
