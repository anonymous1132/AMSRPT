using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AMS.CIM.Caojin.RPTWebApp.Models;
using System.Threading;
using System.Web.Caching;

namespace AMS.CIM.Caojin.RPTWebApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            RegisterCacheEntry();
            Thread t = new Thread(PrepareData);
            t.Start();
            System.Timers.Timer time = new System.Timers.Timer
            {
                Interval = 20 * 60 * 1000
            };
            time.Elapsed += new System.Timers.ElapsedEventHandler(DoWork);//到达时间的时候执行事件；   
            time.AutoReset = true;//设置是执行一次（false）还是一直执行(true)；   
            time.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；   
            // AMSDCMDataTranslator.Program.Main(null);
        }

        private void PrepareData()
        {
            ShareDataEntity.GetSingleEntity();
            ShareDataEntity.GetSingleEntity().Rpt018 = new ReqRpt018ViewModel();
            try
            {
                ShareDataEntity.GetSingleEntity().Rpt018.GetData();
            }
            catch (Exception)
            { }
        }

        public void DoWork(object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                System.Timers.Timer tt = (System.Timers.Timer)source;
                //可防止重复执行程序
                tt.Enabled = false;
                ShareDataEntity.GetSingleEntity().Rpt018.GetData();
                tt.Enabled = true;
            }
            catch (Exception)
            {
            }
        }

        private const string DummyPageUrl = "http://10.8.0.50:8083/";//可访问的页面
        private const string DummyCacheItemKey = "Du";
        private void RegisterCacheEntry()
        {
            if (null != HttpContext.Current.Cache[DummyCacheItemKey]) return;
            HttpContext.Current.Cache.Add(DummyCacheItemKey, "Test", null, DateTime.MaxValue,
            TimeSpan.FromMinutes(5), CacheItemPriority.NotRemovable,
            new CacheItemRemovedCallback(CacheItemRemovedCallback));
        }
        // 缓存项过期时程序模拟点击页面，阻止应用程序结束 
        public void CacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            HitPage();
        }

        // 模拟点击网站网页 
        private void HitPage()
        {
            System.Net.WebClient client = new System.Net.WebClient();
            client.DownloadData(DummyPageUrl);
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.ToString() == DummyPageUrl)
            {
                RegisterCacheEntry();
            }
        }
    }
}
