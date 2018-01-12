using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace HWL.API.Controllers
{
    public class DocController : Controller
    {
        string[] dataType = { "boolean", "string", "int32", "int64", "decimal", "doublue", "float", "datetime" }; //数据类型

        readonly static List<Assembly> serviceAssembly = null;//加载关联的dll

        static DocController()
        {
            serviceAssembly = new List<Assembly>();
            serviceAssembly.Add(Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "bin/HWL.Service.dll"));
            serviceAssembly.Add(Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "bin/HWL.Entity.dll"));
            serviceAssembly.Add(Assembly.LoadFile(AppDomain.CurrentDomain.BaseDirectory + "bin/GMSF.dll"));
        }

        private static Type GetTypeFromAssembly(string fullName)
        {
            foreach (var item in serviceAssembly)
            {
                Type type = item.GetType(fullName);
                if (type != null)
                {
                    return type;
                }
            }
            return null;
        }

        private string getPropertyStr(PropertyInfo pinfo)
        {
            if (pinfo == null) return null;

            string currType = pinfo.PropertyType.Name.ToLower();
            if (currType == "list`1")
            {
                Type[] ts = pinfo.PropertyType.GenericTypeArguments;
                foreach (var tsItem in ts)
                {
                    Type type = GetTypeFromAssembly(tsItem.FullName);
                    if (type == null) continue;
                    object obj = Activator.CreateInstance(type);

                    return string.Format("[{0}]", JsonConvert.SerializeObject(obj));
                }
            }
            else if (!dataType.Contains(currType))
            {
                Type selfType = GetTypeFromAssembly(pinfo.PropertyType.FullName);
                if (selfType == null)
                {
                    if (pinfo.PropertyType.GenericTypeArguments.Length > 0)
                    {
                        selfType = GetTypeFromAssembly(pinfo.PropertyType.GenericTypeArguments[0].FullName);
                    }
                    if (selfType == null)
                        return null;
                }
                Object self = Activator.CreateInstance(selfType);
                return JsonConvert.SerializeObject(self);
            }
            return null;
        }

        // GET: Doc
        public ActionResult Index(String key)
        {
            Type rootType = typeof(DefaultController);

            MethodInfo[] methods = rootType.GetMethods();

            string html = "<script src='/test/js/api_test.js'></script>";
            foreach (MethodInfo method in methods)
            {
                if (method.DeclaringType.Name != rootType.Name) continue;

                //输入参数
                string requestStr = string.Empty;
                var paramArray = method.GetParameters();
                foreach (var paramItem in paramArray)
                {
                    Type[] ts = paramItem.ParameterType.GenericTypeArguments;
                    foreach (var tsItem in ts)
                    {
                        Type type = GetTypeFromAssembly(tsItem.FullName);
                        if (type == null) continue;

                        object obj = Activator.CreateInstance(type);
                        requestStr += JsonConvert.SerializeObject(obj);

                        foreach (var proItem in type.GetProperties())
                        {
                            string proJson = this.getPropertyStr(proItem);
                            if (!string.IsNullOrEmpty(proJson))
                            {
                                string sddd = "";
                            }
                        }
                    }
                }

                //返回参数
                string returnStr = string.Empty;
                Type[] rtp = method.ReturnParameter.ParameterType.GenericTypeArguments;
                foreach (var tsItem in rtp)
                {
                    Type type = GetTypeFromAssembly(tsItem.FullName);
                    if (type == null) continue;

                    object obj = Activator.CreateInstance(type);
                    returnStr += JsonConvert.SerializeObject(obj);

                    foreach (var proItem in type.GetProperties())
                    {
                        string proJson = this.getPropertyStr(proItem);
                        if (!string.IsNullOrEmpty(proJson))
                        {
                            //returnStr = returnStr.Replace("\"" + proItem.Name + "\":null", proJson);
                            returnStr = returnStr.Replace(string.Format("\"{0}\":null", proItem.Name), string.Format("\"{0}\":{1}", proItem.Name, proJson));
                        }
                    }
                }

                //获取描述  
                Object[] obs = method.GetCustomAttributes(typeof(DescriptionAttribute), false);
                string desc = "";
                foreach (DescriptionAttribute record in obs)
                {
                    desc += record.Description;
                }

                if (!string.IsNullOrEmpty(key))
                {
                    if (desc.Contains(key))
                    {
                    }
                    else
                    {
                        continue;
                    }
                }

                html += string.Format("{0} - {1} <a href='/test/index.html' onclick=addReq(\'{0}\',\'{2}\') target='_blank'>测试</a><br /> request:{2} <br /> response:{3} <br /><br /><br />", method.Name, desc, requestStr, returnStr);
            }

            return Content(html);
        }
    }
}