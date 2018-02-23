using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;

namespace HWL.Resx.Models
{
    public class UpfileHandler
    {
        /// <summary>
        /// 文件保存目录路径
        /// </summary>
        static string savePath = ResxConfigManager.UploadDirectory; //"/upload/";

        /// <summary>
        /// 定义允许上传的文件扩展名
        /// </summary>
        static readonly string fileTypes = "gif,jpg,jpeg,png,bmp,apk,amr,wav,mp4";
        /// <summary>
        /// 最大文件大小
        /// </summary>
        static readonly int maxSize = 30 * 1024 * 1024;//默认30M


        /// <summary>
        /// 返回img_url列表
        /// </summary>
        /// <param name="files"></param>
        /// <param name="folder"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static List<UpfileModel> Process(HttpFileCollection files, string folder, out string error)
        {
            error = "";
            var dic = ProcessDic(files, folder, out error);
            if (dic != null && dic.Count > 0)
            {
                return dic.Values.ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(error))
                    error = "文件上传失败";
            }
            return null;
        }

        public static UpfileModel ProcessSig(HttpFileCollection files, string folder, out string error)
        {
            error = "";
            var dic = ProcessDic(files, folder, out error);
            if (dic != null && dic.Count > 0)
            {
                return dic.Values.FirstOrDefault();
            }
            else
            {
                if (string.IsNullOrEmpty(error))
                    error = "文件上传失败";
            }
            return null;
        }

        /// <summary>
        /// 返回《input_name,img_url》
        /// </summary>
        /// <param name="files"></param>
        /// <param name="folder"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static Dictionary<string, UpfileModel> ProcessDic(HttpFileCollection files, string folder, out string error)
        {
            error = "";
            if (files == null || files.Count <= 0)
            {
                error = "请选择文件";
                return null;
            }

            if (string.IsNullOrEmpty(folder))//存储的目录名称
            {
                folder = "default";
            }

            ////检测路径中是否包含除英文，数字，下划线以外的字符
            //folder = System.Text.RegularExpressions.Regex.Replace(folder, @"[^\u4e00-\u9fa5_a-zA-Z0-9]", "");
            //if (folder == "" || folder == null)
            //{
            //    //return Json(new { state = -1, error = "目录名错误" }, JsonRequestBehavior.AllowGet);
            //    error = "目录名错误";
            //    return null;
            //}

            string saveUrl = savePath;
            if (!ResxConfigManager.FileAccessUrl.Contains("localhost"))
            {
                saveUrl = ResxConfigManager.FileAccessUrl + saveUrl;
            }

            saveUrl = saveUrl + folder;//+ "/" + DateTime.Now.ToString("yyyyMM");//根目录/目录名/时间（年/月）
            string dirPath = HttpContext.Current.Server.MapPath(savePath) + folder; //+ "/" + DateTime.Now.ToString("yyyyMM");//根目录/目录名/时间（年/月）
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            //string fileUrl = "";
            ArrayList fileTypeList = ArrayList.Adapter(fileTypes.Split(','));
            Dictionary<string, UpfileModel> resultPaths = new Dictionary<string, UpfileModel>();
            //判断文件是否超过上传大小
            foreach (string item in files)
            {
                var imgFile = HttpContext.Current.Request.Files[item];
                if (imgFile != null && imgFile.InputStream != null && imgFile.InputStream.Length <= maxSize)
                {
                    string newFileName = "";
                    string fileExt = Path.GetExtension(imgFile.FileName).ToLower();
                    newFileName = System.Text.RegularExpressions.Regex.Replace(imgFile.FileName, fileExt, "");
                    newFileName = System.Text.RegularExpressions.Regex.Replace(newFileName, @"[^\u4e00-\u9fa5_a-zA-Z0-9]", "");
                    newFileName = newFileName + DateTime.Now.ToString("yyyyMMddHHmmss", System.Globalization.DateTimeFormatInfo.InvariantInfo) + fileExt;

                    //判断文件的扩展名是否在指定的范围内
                    if (string.IsNullOrEmpty(fileExt) || Array.IndexOf(fileTypes.Split(','), fileExt.Substring(1).ToLower()) == -1)
                    {
                        //return Json(new { state = -2, error = "上传文件扩展名是不允许的扩展名" });
                        error = "上传文件是不允许的扩展名";
                        return resultPaths;
                    }
                    else
                    {
                        if (!resultPaths.ContainsKey(item))//如果不包含Key
                        {
                            try
                            {
                                imgFile.SaveAs(dirPath + "/" + newFileName);
                            }
                            catch (Exception ex)
                            {
                                //return Json(new { state = -2, error = ex.Message });
                                error = ex.Message;
                                return resultPaths;
                            }
                            resultPaths.Add(item, new UpfileModel()
                            {
                                LocalDir = dirPath,
                                LocalPath = dirPath + "/" + newFileName,
                                AccessPath = saveUrl + "/" + newFileName,
                                AccessDir = saveUrl,
                                FileName = newFileName,
                                FileSize = imgFile.InputStream.Length
                            });
                            //resultPaths.Add(item, saveUrl + "/" + newFileName);
                        }
                        else
                        {
                            error = "file的name不能相同";
                            return resultPaths;
                        }
                    }
                }
                else
                {
                    error = "上传文件超过大小 " + maxSize + "M";
                    return resultPaths;
                }
            }

            //return Json(new { state = 1, url = fileUrl, size = imgFile.InputStream.Length }, JsonRequestBehavior.AllowGet);
            //error = "超出上传大小" + maxSize + "M";
            return resultPaths;
        }
    }

    public class UpfileModel
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件上传目录
        /// </summary>
        public string LocalDir { get; set; }
        /// <summary>
        /// 文件上传本地地址
        /// </summary>
        public string LocalPath { get; set; }
        /// <summary>
        /// 文件对外访问地址
        /// </summary>
        public string AccessPath { get; set; }
        /// <summary>
        /// 文件对外访问目录
        /// </summary>
        public string AccessDir { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
    }
}