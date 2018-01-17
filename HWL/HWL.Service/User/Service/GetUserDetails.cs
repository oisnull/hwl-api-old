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
    public class GetUserDetails : GMSF.ServiceHandler<GetUserDetailsRequestBody, GetUserDetailsResponseBody>
    {
        public GetUserDetails(GetUserDetailsRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
            if (this.request.GetUserId <= 0)
            {
                throw new ArgumentNullException("GetUserId");
            }
        }

        public override GetUserDetailsResponseBody ExecuteCore()
        {
            GetUserDetailsResponseBody res = new GetUserDetailsResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var user = db.t_user.Where(u => u.id == this.request.GetUserId).FirstOrDefault();
                if (user == null) throw new Exception("用户不存在");

                res.UserDetailsInfo = new UserDetailsInfo()
                {
                    Id = user.id,
                    NickName = user.name,
                    HeadImage = user.head_image,

                    NameRemark = "",
                    Country = "",
                    Province = "",
                };

                if (res.UserDetailsInfo != null)
                {
                    //获取关系数据
                    t_user_friend fmodel = db.t_user_friend.Where(u => u.user_id == this.request.UserId && u.friend_user_id == this.request.GetUserId).FirstOrDefault();
                    if (fmodel != null)
                    {
                        res.UserDetailsInfo.IsFriend = true;
                        res.UserDetailsInfo.NameRemark = fmodel.friend_user_remark;
                        res.UserDetailsInfo.FirstSpell = fmodel.friend_first_spell;
                    }
                    else
                    {
                        res.UserDetailsInfo.IsFriend = false;
                    }

                    //获取位置
                    if (user.register_country > 0 && user.register_province > 0 && user.register_city > 0 && user.register_district > 0)
                    {
                        //获取地址信息
                        var pos = (from country in db.t_country
                                   join province in db.t_province on country.id equals province.country_id
                                   join city in db.t_city on province.id equals city.province_id
                                   join dist in db.t_district on city.id equals dist.city_id
                                   where country.id == user.register_country && province.id == user.register_province && city.id == user.register_city && dist.id == user.register_district
                                   select new
                                   {
                                       CountryName = country.name,
                                       ProvinceName = province.name,
                                       CityName = city.name,
                                       DistName = dist.name,
                                   }).FirstOrDefault();
                        if (pos != null)
                        {
                            res.UserDetailsInfo.Country = pos.CountryName;
                            res.UserDetailsInfo.Province = pos.ProvinceName;
                        }
                    }
                }
            }

            return res;
        }
    }
}
