using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace HWL.Tools
{
    /// <summary>
    /// 自定义Json格式
    /// </summary>
    public class CustomJsonResult : JsonResult
    {

        /// <summary>
        /// 格式化字符串
        /// </summary>
        public string FormateStr { get; set; }

        /// <summary>
        /// 重写执行视图
        /// </summary>
        /// <param name="context">上下文</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            HttpResponseBase response = context.HttpContext.Response;

            if (string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }

            if (this.Data != null)
            {
                IsoDateTimeConverter dtConverter = new IsoDateTimeConverter { DateTimeFormat = this.FormateStr };
                string jsonString = JsonConvert.SerializeObject(Data, dtConverter);

                //JavaScriptSerializer jss = new JavaScriptSerializer();
                //string jsonString = jss.Serialize(Data);
                //string p = @"\\/Date\((\d+)\)\\/";
                //System.Text.RegularExpressions.MatchEvaluator matchEvaluator = new System.Text.RegularExpressions.MatchEvaluator(this.ConvertJsonDateToDateString);
                //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(p);
                //jsonString = reg.Replace(jsonString, matchEvaluator);
                response.Write(jsonString);
            }
        }

        /// <summary>  
        /// 将Json序列化的时间由/Date(1294499956278)转为字符串 .
        /// </summary>  
        /// <param name="m">正则匹配</param>
        /// <returns>格式化后的字符串</returns>
        private string ConvertJsonDateToDateString(System.Text.RegularExpressions.Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString(FormateStr);
            return result;
        }
    }

    //public class JsonNetResult : JsonResult
    //{
    //    public Newtonsoft.Json.JsonSerializerSettings Settings { get; private set; }

    //    public JsonNetResult()
    //    {
    //        Settings = new Newtonsoft.Json.JsonSerializerSettings
    //        {
    //            ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
    //        };
    //    }

    //    public override void ExecuteResult(ControllerContext context)
    //    {
    //        if (context == null)
    //        {
    //            throw new ArgumentNullException("context");
    //        }
    //        if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
    //        {
    //            throw new InvalidOperationException("JSON GET is not allowed");
    //        }

    //        HttpResponseBase response = context.HttpContext.Response;
    //        response.ContentType = string.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;
    //        if (this.ContentEncoding != null)
    //        {
    //            response.ContentEncoding = this.ContentEncoding;
    //        }
    //        if (this.Data != null)
    //        {
    //            var scriptSerializer = Newtonsoft.Json.JsonSerializer.Create(this.Settings);
    //            using (var sw = new System.IO.StringWriter())
    //            {
    //                scriptSerializer.Serialize(sw, this.Data);
    //                response.Write(sw.ToString());
    //            }
    //        }
    //    }
    //}
}
