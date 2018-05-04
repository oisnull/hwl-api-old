using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HWL.Manage
{
    public class CommonHandler
    {
        public static List<SelectListItem> GetNoticeTypeList()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            SelectListItem item0 = new SelectListItem()
            {
                Text = "公告类型",
                Value ="0",
            };

            SelectListItem item1 = new SelectListItem()
            {
                Value = ((int)NoticeType.Register).ToString(),
                Text = CustomerEnumDesc.GetNoticeType(NoticeType.Register),
            };

            SelectListItem item2 = new SelectListItem()
            {
                Value = ((int)NoticeType.Update).ToString(),
                Text = CustomerEnumDesc.GetNoticeType(NoticeType.Update),
            };

            SelectListItem item3 = new SelectListItem()
            {
                Value = ((int)NoticeType.Other).ToString(),
                Text = CustomerEnumDesc.GetNoticeType(NoticeType.Other),
            };

            items.Add(item0);
            items.Add(item1);
            items.Add(item2);
            items.Add(item3);

            return items;
        }
    }
}