using HWL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Body
{
    //public class SetUserInfoRequestBody
    //{
    //    public int UserId { get; set; }
    //    public string UserName { get; set; }
    //    public string HeadImage { get; set; }
    //    public UserSex UserSex { get; set; }
    //    public string LifeNotes { get; set; }
    //    public List<int> PosIdList { get; set; }
    //}
    public class SetUserInfoResponseBody
    {
        public ResultStatus Status { get; set; }
        public String Symbol { get; set; }
        public String Name { get; set; }
        public String HeadImage { get; set; }
    }

    public class SetUserSymbolRequestBody
    {
        public int UserId { get; set; }
        public string Symbol { get; set; }
    }

    public class SetUserNameRequestBody
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }

    public class SetUserHeadImageRequestBody
    {
        public int UserId { get; set; }
        public string HeadImageUrl { get; set; }
    }

    public class SetUserSexRequestBody
    {
        public int UserId { get; set; }
        public UserSex UserSex { get; set; }
    }

    public class SetUserLifeNotesRequestBody
    {
        public int UserId { get; set; }
        public string LifeNotes { get; set; }
    }

}
