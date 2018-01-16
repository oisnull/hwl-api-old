using HWL.Entity;
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
            if (this.request.UserPos == null)
            {
                throw new ArgumentNullException("UserPos");
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
                t_country country = db.t_country.Where(c => c.name == this.request.UserPos.Country).FirstOrDefault();
                if (country == null)
                {
                    country = new t_country()
                    {
                        id = 0,
                        name = this.request.UserPos.Country,
                    };
                    db.t_country.Add(country);
                    db.SaveChanges();
                }

                t_province province = db.t_province.Where(p => p.name == this.request.UserPos.Province).FirstOrDefault();
                if (province == null)
                {
                    province = new t_province()
                    {
                        id = 0,
                        country_id = country.id,
                        name = this.request.UserPos.Province,
                    };
                    db.t_province.Add(province);
                    db.SaveChanges();
                }

                t_city city = db.t_city.Where(c => c.name == this.request.UserPos.City).FirstOrDefault();
                if (city == null)
                {
                    city = new t_city()
                    {
                        id = 0,
                        province_id = province.id,
                        name = this.request.UserPos.City,
                    };
                    db.t_city.Add(city);
                    db.SaveChanges();
                }

                t_district district = db.t_district.Where(p => p.name == this.request.UserPos.District).FirstOrDefault();
                if (district == null)
                {
                    district = new t_district()
                    {
                        id = 0,
                        city_id = city.id,
                        name = this.request.UserPos.District,
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
                                                            u.pos_details == this.request.UserPos.Details
                                                        ).FirstOrDefault();
                if (upos == null)
                {
                    //向用户位置表中加入数据
                    upos = new t_user_pos()
                    {
                        id = 0,
                        create_date = DateTime.Now,
                        update_date = DateTime.Now,
                        geohash_key = Geohash.Encode(this.request.UserPos.Latitude, this.request.UserPos.Longitude),
                        lat = this.request.UserPos.Latitude,
                        lon = this.request.UserPos.Longitude,
                        pos_details = this.request.UserPos.Details,
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

                //将用户加入到附近组中
                groupAction.SaveGroupUser(res.UserGroupGuid, upos.user_id);
            }

            return res;
        }
    }
}
