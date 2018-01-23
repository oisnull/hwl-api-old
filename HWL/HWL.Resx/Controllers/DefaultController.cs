using HWL.Entity;
using HWL.Resx.Models;
using HWL.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HWL.Resx.Controllers
{
    [Route("resx/{action}")]
    public class DefaultController : ApiController
    {
        public ResponseResult Image(string token = null, int resxType = 7)
        {
            bool succ = this.checkRequestParam(token);
            if (!succ)
            {
                return new ResponseResult() { Status = ResultStatus.Failed, Message = "凭证验证失败" };
            }

            ResxHandler resx = new ResxHandler(Request, (ResxType)resxType);
            if (!resx.IsMultipartContent())
            {
                return new ResponseResult() { Status = ResultStatus.Failed, Message = "资源格式错误" };
            }
            resx.LoadStream();
            if (resx.IsStreamEmpty())
            {
                return new ResponseResult() { Status = ResultStatus.Failed, Message = "资源数据不能为空" };
            }

            return null;
        }

        private bool checkRequestParam(string token)
        {
            RequestValidate requestValidate = new RequestValidate();
            return requestValidate.CheckToken(token);
        }
    }

    public class ResponseResult
    {
        public ResultStatus Status { get; set; }
        public string Message { get; set; }
    }

    public class ResxHandler
    {
        private string rootDir;
        private HttpRequestMessage request;
        private MultipartFormDataMemoryStreamProvider provider;

        public ResxHandler(HttpRequestMessage request, ResxType resxType = ResxType.Other)
        {
            this.request = request;
            rootDir = AppDomain.CurrentDomain.BaseDirectory + "/upload/" + CustomerEnumDesc.GetResxTypePath(resxType);
        }

        public bool IsMultipartContent()
        {
            //检查该请求是否含有multipart/form-data
            if (!request.Content.IsMimeMultipartContent())
            {
                return false;
                //throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            return true;
        }

        public async void LoadStream()
        {
            provider = new MultipartFormDataMemoryStreamProvider();
            try
            {
                await request.Content.ReadAsMultipartAsync(provider);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsStreamEmpty()
        {
            if (provider == null ||
                provider.FileData == null ||
                provider.FileData.Count <= 0)
                return true;
            return false;
        }

        public void SaveStream()
        {
            if (!IsStreamEmpty())
            {
                foreach (MultipartFileData file in provider.FileData)
                {
                    
                    //接收文件
                    //Trace.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
                    //Trace.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名
                    var stream = ((MultipartFileDataStream)file).Stream;
                    using (StreamWriter sw = new StreamWriter(Path.Combine(rootDir, file.Headers.ContentDisposition.FileName)))
                    {
                        stream.CopyTo(sw.BaseStream);
                        sw.Flush();
                    }
                }
            }
        }

    }

    //    [HttpPost]
    //    public async Task<Dictionary<string, string>> Index(string token, int resxType)
    //    {
    //        if (!Request.Content.IsMimeMultipartContent())
    //        {
    //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
    //        }
    //        Dictionary<string, string> dic = new Dictionary<string, string>();
    //        string root = AppDomain.CurrentDomain.BaseDirectory;
    //        var provider = new MultipartFormDataMemoryStreamProvider();
    //        try
    //        {
    //            // Read the form data.
    //            await Request.Content.ReadAsMultipartAsync(provider);

    //            // This illustrates how to get the file names.
    //            foreach (MultipartFileData file in provider.FileData)
    //            {
    //                //接收文件
    //                Trace.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
    //                Trace.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名
    //                var stream = ((MultipartFileDataStream)file).Stream;
    //                using (StreamWriter sw = new StreamWriter(Path.Combine(root, file.Headers.ContentDisposition.FileName)))
    //                {
    //                    stream.CopyTo(sw.BaseStream);
    //                    sw.Flush();
    //                }
    //            }
    //            //TODO:这样做直接就将文件存到了指定目录下，暂时不知道如何实现只接收文件数据流但并不保存至服务器的目录下，由开发自行指定如何存储，比如通过服务存到图片服务器
    //            foreach (var key in provider.FormData.AllKeys)
    //            {//接收FormData
    //                dic.Add(key, provider.FormData[key]);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            throw ex;
    //        }
    //        return dic;
    //    }
    //}
}
