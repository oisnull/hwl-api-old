using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HWL.Manage.Service
{
    public class AppService : BaseService
    {
        public List<AppExt> GetAppVersionList()
        {
            return dbContext.t_app_version.Select(v => new AppExt
            {
                Id = v.id,
                DownloadUrl = v.download_url,
                Name = v.app_name,
                PublishTime = v.publish_time,
                Version = v.app_version,
            }).ToList();
        }

        public AppExt GetAppVersionInfo(int id)
        {
            if (id <= 0) return null;

            return dbContext.t_app_version.Where(v => v.id == id).Select(v => new AppExt
            {
                Id = v.id,
                DownloadUrl = v.download_url,
                Name = v.app_name,
                PublishTime = v.publish_time,
                Version = v.app_version,
            }).FirstOrDefault();
        }

        public string GetAppLastVersionUrl()
        {
            return dbContext.t_app_version.OrderByDescending(v => v.id).Select(v => v.download_url).FirstOrDefault();
        }

        public int DeleteAppVersion(int id, out string error)
        {
            error = "";
            if (id <= 0)
            {
                error = "参数错误";
                return 0;
            }

            var model = dbContext.t_app_version.Where(a => a.id == id).FirstOrDefault();
            if (model == null)
            {
                error = "数据不存在";
                return 0;
            }

            try
            {
                dbContext.t_app_version.Remove(model);
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }

        public int AppVersionAction(AppExt model, out string error)
        {
            error = "";
            if (model == null)
            {
                error = "数据不能为空";
                return 0;
            }
            if (string.IsNullOrEmpty(model.Name))
            {
                error = "app 名称不能为空";
                return 0;
            }
            if (string.IsNullOrEmpty(model.Version))
            {
                error = "app 版本号不能为空";
                return 0;
            }
            if (string.IsNullOrEmpty(model.DownloadUrl))
            {
                error = "app 下载地址不能为空";
                return 0;
            }

            t_app_version entity = null;
            if (model.Id > 0)
            {
                entity = dbContext.t_app_version.Where(v => v.id == model.Id).FirstOrDefault();
                if (entity == null)
                {
                    error = "数据不存在";
                    return 0;
                }
            }
            else
            {
                entity = new t_app_version();
                entity.publish_time = DateTime.Now;
                dbContext.t_app_version.Add(entity);
            }
            entity.app_name = model.Name;
            entity.app_version = model.Version;
            entity.download_url = model.DownloadUrl;
            entity.update_time = DateTime.Now;

            try
            {
                dbContext.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 0;
            }
        }
    }
}
