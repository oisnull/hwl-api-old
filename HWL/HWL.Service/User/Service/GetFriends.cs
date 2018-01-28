using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.User.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class GetFriends : GMSF.ServiceHandler<GetFriendsRequestBody, GetFriendsResponseBody>
    {
        public GetFriends(GetFriendsRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();
            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
        }

        public override GetFriendsResponseBody ExecuteCore()
        {
            GetFriendsResponseBody res = new GetFriendsResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var users = (from f in db.t_user_friend
                             join u in db.t_user on f.friend_user_id equals u.id
                             where u.status == UserStatus.Normal && f.user_id == this.request.UserId
                             select new
                             {
                                 CountryId = u.register_country,
                                 ProvinceId = u.register_province,
                                 NameRemark = f.friend_user_remark,
                                 NickName = u.name,
                                 FirstSpell = f.friend_first_spell,
                                 Id = f.friend_user_id,
                                 HeadImage = u.head_image,
                                 Symbol = u.symbol,
                             }).ToList();
                if (users == null || users.Count <= 0) return res;

                var countryIds = users.Select(u => u.CountryId).Distinct().ToList();
                var provinceIds = users.Select(u => u.ProvinceId).Distinct().ToList();

                var countryList = db.t_country.Where(c => countryIds.Contains(c.id)).Select(c => new { CountryId = c.id, CountryName = c.name }).ToList();
                var provinceList = db.t_province.Where(c => provinceIds.Contains(c.id)).Select(c => new { ProvinceId = c.id, ProvinceName = c.name }).ToList();

                res.UserFriendInfos = new List<UserFriendInfo>();
                users.ForEach(u =>
                {
                    string countryName = countryList != null && countryList.Count > 0 ? countryList.Where(c => c.CountryId == u.CountryId).Select(c => c.CountryName).FirstOrDefault() : "";
                    string provinceName = provinceList != null && provinceList.Count > 0 ? provinceList.Where(c => c.ProvinceId == u.ProvinceId).Select(c => c.ProvinceName).FirstOrDefault() : "";

                    res.UserFriendInfos.Add(new UserFriendInfo()
                    {
                        Id = u.Id,
                        Country = countryName,
                        Province = provinceName,
                        HeadImage = u.HeadImage,
                        NameRemark = u.NameRemark,
                        ShowName = u.NickName,
                        Symbol = u.Symbol,
                    });
                });
            }

            return res;
        }
    }
}
