using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Manage.Service
{
    public class NoticeService : BaseService
    {
        //public List<NoticeManageModel> GeNoticeList()
        //{
        //    return dbContext.t_notice.Select(v => new NoticeManageModel
        //    {
        //        Id = v.id,
        //        CreateTime = v.create_time,
        //        Ncontent = v.ncontent,
        //        NoticeType = v.notice_type,
        //        Title = v.title,
        //    }).ToList();
        //}
        //public NoticeManageModel GetNoticeInfo(int id)
        //{
        //    if (id <= 0) return null;

        //    return dbContext.t_notice.Where(v => v.id == id).Select(v => new NoticeManageModel
        //    {
        //        Id = v.id,
        //        CreateTime = v.create_time,
        //        Ncontent = v.ncontent,
        //        NoticeType = v.notice_type,
        //        Title = v.title,
        //    }).FirstOrDefault();
        //}

        //public int NoticeAction(NoticeManageModel model, out string error)
        //{
        //    error = "";
        //    if (model == null)
        //    {
        //        error = "数据不能为空";
        //        return 0;
        //    }
        //    if (model.NoticeType <= 0)
        //    {
        //        error = "公告类型不能为空";
        //        return 0;
        //    }
        //    if (string.IsNullOrEmpty(model.Title))
        //    {
        //        error = "标题不能为空";
        //        return 0;
        //    }
        //    if (string.IsNullOrEmpty(model.Ncontent))
        //    {
        //        error = "内容不能为空";
        //        return 0;
        //    }

        //    t_notice entity = null;
        //    if (model.Id > 0)
        //    {
        //        entity = dbContext.t_notice.Where(v => v.id == model.Id).FirstOrDefault();
        //        if (entity == null)
        //        {
        //            error = "数据不存在";
        //            return 0;
        //        }
        //    }
        //    else
        //    {
        //        entity = new t_notice();
        //        entity.create_time = DateTime.Now;
        //        dbContext.t_notice.Add(entity);
        //    }
        //    entity.ncontent = model.Ncontent;
        //    entity.notice_type = model.NoticeType;
        //    entity.title = model.Title;

        //    try
        //    {
        //        dbContext.SaveChanges();
        //        return 1;
        //    }
        //    catch (Exception ex)
        //    {
        //        error = ex.Message;
        //        return 0;
        //    }
        //}
    }
}
