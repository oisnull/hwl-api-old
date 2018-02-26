using GMSF.Model;
using HWL.Entity;
using HWL.Resx.Models;
using HWL.Service;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace HWL.Resx.Controllers
{
    [Route("resx/{action}")]
    public class DefaultController : BaseApiController
    {
        LogAction log = new LogAction("api-" + System.DateTime.Now.ToString("yyyyMMdd") + ".txt");

        [HttpPost]
        public async Task<Response<ResxResult>> Image(string token = null, ResxType resxType = ResxType.Other)
        {
            var ret = this.CheckToken(token);
            if (!ret.Item1)
            {
                return GetResult(GMSF.ResponseResult.FAILED, "TOKEN 验证失败");
            }

            ResxHandler resx = new ResxHandler(Request, new ResxModel()
            {
                UserId = ret.Item2,
                ResxType = resxType,
                ResxSize = ResxConfigManager.IMAGE_MAX_SIZE,
                ResxTypes = ResxConfigManager.IMAGE_FILE_TYPES
            });

            if (!resx.IsMultipartContent())
            {
                return GetResult(GMSF.ResponseResult.FAILED, "资源格式错误");
            }
            try
            {
                var responseResult = await resx.SaveStream();
                var res = GetResult(GMSF.ResponseResult.SUCCESS, null, responseResult);
                log.WriterLog(Newtonsoft.Json.JsonConvert.SerializeObject(res));
                return res;
            }
            catch (Exception ex)
            {
                log.WriterLog(ex.Message);
                return GetResult(GMSF.ResponseResult.FAILED, ex.Message);
            }

        }


        //[HttpPost]
        //public Response<UpImageResponseBody> Image(string token = null, int resxType = 7)
        //{
        //    var ret = this.CheckToken(token);
        //    if (!ret.Item1)
        //    {
        //        return GetResult(GMSF.ResponseResult.FAILED, "TOKEN 验证失败");
        //    }

        //    //保存上传数据
        //    string error = "";
        //    string saveDir = CustomerEnumDesc.GetResxTypePath((ResxType)resxType, ret.Item2);
        //    var fileModel = UpfileHandler.ProcessSig(HttpContext.Current.Request.Files, saveDir, out error);
        //    if (fileModel == null)
        //    {
        //        return GetResult(GMSF.ResponseResult.FAILED, error);
        //    }
        //    //var response = GetResult(GMSF.ResponseResult.SUCCESS, null, new UpImageResponseBody() { AccessUrl = fileModel.AccessPath, FileSize = fileModel.FileSize });

        //    //log.WriterLog(Newtonsoft.Json.JsonConvert.SerializeObject(response));
        //    //return response;
        //    return null;
        //}

        //[HttpPost]
        //public async Task<Response<UpImageResponseBody>> Image(string token = null, int resxType = 7)
        //{
        //    var ret = this.CheckToken(token);
        //    if (!ret.Item1)
        //    {
        //        return GetResult(GMSF.ResponseResult.FAILED, "TOKEN 验证失败");
        //    }

        //    ResxHandler resx = new ResxHandler(Request, (ResxType)resxType);
        //    if (!resx.IsMultipartContent())
        //    {
        //        return GetResult(GMSF.ResponseResult.FAILED, "资源格式错误");
        //    }
        //    try
        //    {
        //        await resx.SaveStream();
        //    }
        //    catch (Exception ex)
        //    {
        //        return GetResult(GMSF.ResponseResult.FAILED, ex.Message);
        //    }

        //    //return GetResult(GMSF.ResponseResult.SUCCESS, null, new UpImageResponseBody() { AccessUrls = resx.GetAccessUrls() });
        //    return null;
        //}

        //[HttpPost]
        //public async Task<Dictionary<string, string>> Index()
        //{
        //    if (!Request.Content.IsMimeMultipartContent())
        //    {
        //        throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //    }
        //    Dictionary<string, string> dic = new Dictionary<string, string>();
        //    string root = AppDomain.CurrentDomain.BaseDirectory;
        //    var provider = new MultipartFormDataMemoryStreamProvider();
        //    try
        //    {
        //        // Read the form data.
        //        await Request.Content.ReadAsMultipartAsync(provider);

        //        // This illustrates how to get the file names.
        //        foreach (MultipartFileData file in provider.FileData)
        //        {
        //            //接收文件
        //            //Trace.WriteLine(file.Headers.ContentDisposition.FileName);//获取上传文件实际的文件名
        //            //Trace.WriteLine("Server file path: " + file.LocalFileName);//获取上传文件在服务上默认的文件名
        //            var stream = ((MultipartFileDataStream)file).Stream;
        //            using (StreamWriter sw = new StreamWriter(Path.Combine(root, file.Headers.ContentDisposition.FileName)))
        //            {
        //                stream.CopyTo(sw.BaseStream);
        //                sw.Flush();
        //            }
        //        }
        //        //TODO:这样做直接就将文件存到了指定目录下，暂时不知道如何实现只接收文件数据流但并不保存至服务器的目录下，由开发自行指定如何存储，比如通过服务存到图片服务器
        //        foreach (var key in provider.FormData.AllKeys)
        //        {//接收FormData
        //            dic.Add(key, provider.FormData[key]);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return dic;
        //}

        //private void btRequest_Click(object sender, EventArgs e)
        //{
        //    this.txtResponse.Text = string.Empty;
        //    using (HttpClient client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/" + this.cmbResponseContentType.Text.ToLower()));//设定要响应的数据格式
        //        using (var content = new MultipartFormDataContent())//表明是通过multipart/form-data的方式上传数据
        //        {
        //            var formDatas = this.GetFormDataByteArrayContent(this.GetNameValueCollection(this.gv_FormData));//获取键值集合对应的ByteArrayContent集合
        //            var files = this.GetFileByteArrayContent(this.GetHashSet(this.gv_File));//获取文件集合对应的ByteArrayContent集合
        //            Action<List<ByteArrayContent>> act = (dataContents) =>
        //            {//声明一个委托，该委托的作用就是将ByteArrayContent集合加入到MultipartFormDataContent中
        //                foreach (var byteArrayContent in dataContents)
        //                {
        //                    content.Add(byteArrayContent);
        //                }
        //            };
        //            act(formDatas);//执行act
        //            act(files);//执行act
        //            try
        //            {
        //                var result = client.PostAsync(this.txtUrl.Text, content).Result;//post请求
        //                this.txtResponse.Text = result.Content.ReadAsStringAsync().Result;//将响应结果显示在文本框内
        //            }
        //            catch (Exception ex)
        //            {
        //                this.txtResponse.Text = ex.ToString();//将异常信息显示在文本框内
        //            }
        //        }
        //    }
        //}
        ///// <summary>
        ///// 获取文件集合对应的ByteArrayContent集合
        ///// </summary>
        ///// <param name="files"></param>
        ///// <returns></returns>
        //private List<ByteArrayContent> GetFileByteArrayContent(HashSet<string> files)
        //{
        //    List<ByteArrayContent> list = new List<ByteArrayContent>();
        //    foreach (var file in files)
        //    {
        //        var fileContent = new ByteArrayContent(File.ReadAllBytes(file));
        //        fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //        {
        //            FileName = Path.GetFileName(file)
        //        };
        //        list.Add(fileContent);
        //    }
        //    return list;
        //}
        ///// <summary>
        ///// 获取键值集合对应的ByteArrayContent集合
        ///// </summary>
        ///// <param name="collection"></param>
        ///// <returns></returns>
        //private List<ByteArrayContent> GetFormDataByteArrayContent(NameValueCollection collection)
        //{
        //    List<ByteArrayContent> list = new List<ByteArrayContent>();
        //    foreach (var key in collection.AllKeys)
        //    {
        //        var dataContent = new ByteArrayContent(Encoding.UTF8.GetBytes(collection[key]));
        //        dataContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //        {
        //            Name = key
        //        };
        //        list.Add(dataContent);
        //    }
        //    return list;
        //}
    }
}
