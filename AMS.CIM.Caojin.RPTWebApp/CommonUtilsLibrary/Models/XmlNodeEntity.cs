using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CommonUtilsLibrary.Models
{
    /// <summary>
    /// 只支持String属性，List/String字段
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XmlNodeEntity<T> where T:new ()
    {
        public List<T> Nodes { get; set; } = new List<T>();

        public string GetNodeName { get { return new T().GetType().Name; } }

        public string GetSingleNodeXmlCtx(T t)
        {
            string nodeName = GetNodeName;
            string head =GetNodeName;
            PropertyInfo[] propertys = t.GetType().GetProperties();
            foreach (var pi in propertys)
            {
                var name = pi.Name;//获得属性的名字,后面就可以根据名字判断来进行些自己想要的操作
                var value = pi.GetValue(t, null);//用pi.GetValue获得值
                var type = value?.GetType() ?? typeof(object);//获得属性的类型
                if (type == typeof(String))
                {
                    head += string.Format(" {0}=\"{1}\"",name,value);
                }
            }
            string ctx = "";
            FieldInfo[] fields= t.GetType().GetFields(BindingFlags.Public|BindingFlags.DeclaredOnly|BindingFlags.Instance);
            foreach (var fi in fields)
            {
                var name = fi.Name;
                var value = fi.GetValue(t);
                var type = value?.GetType() ?? typeof(object);
                if (type.GetGenericTypeDefinition() == typeof(List<>))
                {

                }
                else if (type.IsValueType||type==typeof(String))
                {
                    ctx += string.Format("<{0}>{1}</{0}>",name,value);
                }
               
            }
           
            return string.Format("<{0}>\n{1}\n</{2}>\n",head,ctx,nodeName);
        }
    }
}
