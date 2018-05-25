using HWL.Entity;
using HWL.Entity.Extends;
using HWL.Service.User.Body;
using HWL.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.User.Service
{
    public class SetUserPos : GMSF.ServiceHandler<SetUserPosRequestBody, SetUserPosResponseBody>
    {
        double lat = 0;
        double lon = 0;

        public SetUserPos(SetUserPosRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
            if (string.IsNullOrEmpty(this.request.Latitude) || string.IsNullOrEmpty(this.request.Longitude))
            {
                throw new Exception("经度和纬度不能为空");
            }

            double.TryParse(this.request.Latitude, out lat);
            double.TryParse(this.request.Longitude, out lon);

            if (lat <= 0 || lon <= 0)
            {
                throw new Exception("经度和纬度的值错误");
            }
        }

        public override SetUserPosResponseBody ExecuteCore()
        {
            SetUserPosResponseBody res = new SetUserPosResponseBody();

            //{
            //    "UserId": 1,
            //    "UserPos": {
            //        "Country": "中国",
            //        "Province": "上海市",
            //        "City": "上海市",
            //        "District": "闵行区",
            //        "Details": "上海市闵行区浦申路靠近上海闵行区金色阳光世博幼儿园",
            //        "Latitude": 31.0732898712158,
            //        "Longitude": 121.507202148438
            //    }
            //}

            using (HWLEntities db = new HWLEntities())
            {
                //检测是否存在country province city district 没有就添加
                t_country country = db.t_country.Where(c => c.name == this.request.Country).FirstOrDefault();
                if (country == null)
                {
                    country = new t_country()
                    {
                        id = 0,
                        name = this.request.Country,
                    };
                    db.t_country.Add(country);
                    db.SaveChanges();
                }

                t_province province = db.t_province.Where(p => p.name == this.request.Province).FirstOrDefault();
                if (province == null)
                {
                    province = new t_province()
                    {
                        id = 0,
                        country_id = country.id,
                        name = this.request.Province,
                    };
                    db.t_province.Add(province);
                    db.SaveChanges();
                }

                t_city city = db.t_city.Where(c => c.name == this.request.City).FirstOrDefault();
                if (city == null)
                {
                    city = new t_city()
                    {
                        id = 0,
                        province_id = province.id,
                        name = this.request.City,
                    };
                    db.t_city.Add(city);
                    db.SaveChanges();
                }

                t_district district = db.t_district.Where(p => p.name == this.request.District).FirstOrDefault();
                if (district == null)
                {
                    district = new t_district()
                    {
                        id = 0,
                        city_id = city.id,
                        name = this.request.District,
                    };
                    db.t_district.Add(district);
                    db.SaveChanges();
                }

                //检测用户是否已经存在当前位置信息(条件,用户id,位置id,位置详情)
                t_user_pos upos = db.t_user_pos.Where(u => u.user_id == this.request.UserId &&
                                                            u.country_id == country.id &&
                                                            u.province_id == province.id &&
                                                            u.city_id == city.id &&
                                                            u.district_id == district.id &&
                                                            u.pos_details == this.request.Details
                                                        ).FirstOrDefault();
                if (upos == null)
                {
                    //向用户位置表中加入数据
                    upos = new t_user_pos()
                    {
                        id = 0,
                        create_date = DateTime.Now,
                        update_date = DateTime.Now,
                        geohash_key = Geohash.Encode(lat, lon),
                        lat = lat,
                        lon = lon,
                        pos_details = this.request.Details,
                        user_id = this.request.UserId,
                        city_id = city.id,
                        country_id = country.id,
                        district_id = district.id,
                        province_id = province.id,
                    };
                    db.t_user_pos.Add(upos);
                }
                else
                {
                    upos.update_date = DateTime.Now;
                }

                db.SaveChanges();
                res.Status = ResultStatus.Success;
                res.UserPosId = upos.id;

                //保存用户的位置到redis
                Redis.UserAction userAction = new Redis.UserAction();
                userAction.SavePos(upos.user_id, upos.lon, upos.lat);

                //获取当前位置附近的组
                Redis.GroupAction groupAction = new Redis.GroupAction();
                res.UserGroupGuid = groupAction.GetNearGroupGuid(upos.lon, upos.lat);
                if (string.IsNullOrEmpty(res.UserGroupGuid))
                {
                    //如果没有组数据，创建一个组
                    res.UserGroupGuid = groupAction.CreateGroupPos(upos.lon, upos.lat);
                }
                else
                {
                    if (this.request.LastGroupGuid == res.UserGroupGuid)
                    {
                        return res;
                    }
                }

                //将用户加入到组中
                groupAction.SaveGroupUser(res.UserGroupGuid, upos.user_id);

                //返回组用户列表
                List<int> userIds = groupAction.GetGroupUserIds(res.UserGroupGuid);
                if (userIds == null || userIds.Count <= 0) return res;

                res.GroupUserInfos = db.t_user.Where(u => userIds.Contains(u.id))
                    .Select(u => new GroupUserInfo()
                    {
                        GroupGuid = res.UserGroupGuid,
                        UserId = u.id,
                        UserName = u.name,
                        UserHeadImage = u.head_image,
                    }).ToList();

                //发送mq welcome组消息给用户
                RabbitMQ.android_message.AndroidChatMessage.SendNearGroupWelcome(this.request.UserId, res.UserGroupGuid, res.GroupUserInfos.Select(g => g.UserHeadImage).Take(9).ToList(), "欢迎加入HWL附近聊天组");
            }

            return res;
        }
    }
}
