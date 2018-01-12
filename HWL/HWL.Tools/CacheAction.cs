using System;
using System.Collections;
using System.Web;
using System.Web.Caching;

namespace HWL.Tools
{
    /// <summary>
    /// 提供Cache缓存操作
    /// </summary>
    public class CacheAction
    {
        private static readonly Cache _cache;

        static CacheAction()
        {
            HttpContext current = HttpContext.Current;
            if (current != null)
            {
                _cache = current.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// 返回缓存列表
        /// </summary>
        /// <returns></returns>
        public static ArrayList GetKeys()
        {
            IDictionaryEnumerator enumerator = _cache.GetEnumerator();
            ArrayList list = new ArrayList();
            while (enumerator.MoveNext())
            {
                list.Add(enumerator.Key);
            }
            return list;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        public static void Clear()
        {
            foreach (string str in GetKeys())
            {
                _cache.Remove(str);
            }
        }

        /// <summary>
        /// 缓存是否存在
        /// </summary>
        /// <param name="CacheKey">索引键值</param>
        /// <returns></returns>
        public static bool Exists(string CacheKey)
        {
            if (_cache[CacheKey] == null)
                return false;
            return true;
        }

        /// <summary>
        /// 获取当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey">索引键值</param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            return _cache[CacheKey];
        }

        /// <summary>
        /// 设置当前应用程序指定CacheKey的Cache值
        /// </summary>
        /// <param name="CacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        public static void SetCache(string CacheKey, object objObject)
        {
            _cache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置以缓存依赖的方式缓存数据
        /// </summary>
        /// <param name="CacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        /// <param name="cacheDepen">依赖对象</param>
        public static void SetCache(string CacheKey, object objObject, CacheDependency dep)
        {
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(
                CacheKey,
                objObject,
                dep,
                Cache.NoAbsoluteExpiration, //从不过期
                Cache.NoSlidingExpiration, //禁用可调过期
                CacheItemPriority.Default,
                null);
        }

        /// <summary>
        /// 设置以缓存依赖的方式缓存数据
        /// </summary>
        /// <param name="CacheKey">索引键值</param>
        /// <param name="objObject">缓存对象</param>
        /// <param name="depPath">依赖对象所在的路径</param>
        public static void SetCache(string CacheKey, object objObject, string depPath)
        {
            //依赖文件的变化来更新缓存
            CacheDependency dep = new CacheDependency(depPath);
            Cache objCache = HttpRuntime.Cache;
            objCache.Insert(
                CacheKey,
                objObject,
                dep,
                Cache.NoAbsoluteExpiration, //从不过期
                Cache.NoSlidingExpiration, //禁用可调过期
                CacheItemPriority.Default,
                null);
        }

        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="CacheKey">当前应用程序指定CacheKey</param>
        public static void RemoveCache(string CacheKey)
        {
            _cache.Remove(CacheKey);
        }

        /// <summary>
        /// 设定绝对的过期时间
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="seconds">超过多少秒后过期</param>
        public static void SetCache(string CacheKey, object objObject, long Seconds)
        {
            _cache.Insert(CacheKey, objObject, null, System.DateTime.Now.AddSeconds(Seconds), TimeSpan.Zero);
        }
    }

    /// <summary>
    /// 用缓存替换Seesion，用于不同域名之间共享
    /// </summary>
    public class SessionCache
    {
        const string SESSION_COOKIE_ID = "CURRENTUSERCOOKIEID";

        /// <summary>
        /// 获取设置SessionID值
        /// </summary>
        /// <returns></returns>
        public string CreatSessionCookie()
        {
            if (HttpContext.Current.Request.Cookies[SESSION_COOKIE_ID] != null)
            {
                return HttpContext.Current.Request.Cookies[SESSION_COOKIE_ID].Value.ToString();
            }
            else
            {
                Guid guid = Guid.NewGuid();
                HttpCookie cokie = new HttpCookie(SESSION_COOKIE_ID);
                cokie.Value = guid.ToString();
                cokie.Expires = System.DateTime.Now.AddHours(12);
                //if (!ConfigManager.MainDomain.Contains("localhost"))//本地测试时取消域名间session共享
                //{
                //    cokie.Domain = ConfigManager.MainDomain;
                //}
                cokie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cokie);
                return guid.ToString();
            }
        }

        public object this[string key]
        {
            get
            {
                key = CreatSessionCookie() + "_" + key;
                return CacheAction.GetCache(key);
            }
            set
            {
                if (value == null)
                {
                    CacheAction.RemoveCache(key);
                }
                else
                {
                    SetSession(key, value);
                }
            }
        }

        public void SetSession(string key, object value)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Key is Null or Epmty");
            }
            key = CreatSessionCookie() + "_" + key;
            CacheAction.SetCache(key, value);
        }

        public void RemoveSession(string key)
        {
            key = CreatSessionCookie() + "_" + key;

            CacheAction.RemoveCache(key);
        }

        public string SessionId
        {
            get { return CreatSessionCookie(); }
        }

    }
}
