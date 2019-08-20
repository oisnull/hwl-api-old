using HWL.Entity;
using HWL.Entity.Extends;
using HWL.IMClient;
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
        HWLEntities db;
        Redis.UserAction userAction;
        Redis.GroupAction groupAction;
        bool isExistInGroup = false;

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

            if (this.request.Latitude <= 0 && this.request.Latitude <= 0)
            {
                throw new Exception("经度或者纬度的值是错误的");
            }
        }
		
		//{
		//	"UserId": 1,
		//	"LastGroupGuid": null,
		//	"Country": "中国",
		//	"Province": "上海市",
		//	"City": "上海市",
		//	"District": "闵行区",
		//	"Street": "金色阳光世博幼儿园",
		//	"Details": "上海市闵行区浦申路靠近上海闵行区金色阳光世博幼儿园",
		//	"Latitude":31.0732898712158,
		//	"Longitude": 121.507202148438
		//}

        public override SetUserPosResponseBody ExecuteCore()
        {
            db = new HWLEntities();
            userAction = new Redis.UserAction();
            groupAction = new Redis.GroupAction();

            t_user_pos upos = this.SavePos();
            string groupGuid = GetGroupGuid(upos);

            SetUserPosResponseBody res = new SetUserPosResponseBody()
            {
                Status = ResultStatus.Success,
                UserPosId = upos.id,
                UserGroupGuid = groupGuid,
                GroupUserInfos = null
            };

            if (!isExistInGroup)
            {
                res.GroupUserInfos = GetGroupUsers(groupGuid);
                string userName = res.GroupUserInfos?.Where(g => g.UserId == request.UserId).Select(g => g.UserName).FirstOrDefault();
                IMClientV.INSTANCE.SendSystemMessage((ulong)request.UserId, userName, groupGuid);
            }

            return res;
        }

        public List<UserSecretInfo> GetGroupUsers(string groupGuid)
        {
            List<int> userIds = groupAction.GetGroupUserIds(groupGuid);
            if (userIds == null || userIds.Count <= 0) return null;

            return db.t_user.Where(u => userIds.Contains(u.id))
                .Select(u => new UserSecretInfo()
                {
                    UserId = u.id,
                    UserName = u.name,
                    UserImage = u.head_image,
                }).ToList();
        }

        public string GetGroupGuid(t_user_pos upos)
        {
            //1,save user lat,lon
            //2,check group is exist in lat,lon
            //3,if non-exist and create group by lat,lon and save user and back group guid else directly back group guid
            //4,if current group guid is not equal request.lastGroupGuid and delete user from request.lastGroupGuid

            userAction.SavePos(upos.user_id, upos.lon, upos.lat);

            string groupGuid = groupAction.GetNearGroupGuid(upos.lon, upos.lat);
            if (groupGuid == null)
            {
                groupGuid = groupAction.CreateNearGroupPos(upos.lon, upos.lat);
            }
            else
            {
                //if user exist in group indicate that seted pos
                if (groupAction.ExistsInGroup(groupGuid, this.request.UserId))
                {
                    isExistInGroup = true;
                    return groupGuid;
                }
            }
            groupAction.SaveGroupUser(groupGuid, upos.user_id);

            if (!string.IsNullOrEmpty(this.request.LastGroupGuid) && this.request.LastGroupGuid != groupGuid)
                groupAction.DeleteGroupUser(this.request.LastGroupGuid, upos.user_id);

            return groupGuid;
        }

        public t_user_pos SavePos()
        {
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

            t_user_pos upos = db.t_user_pos.Where(u => u.user_id == this.request.UserId &&
                                                        u.country_id == country.id &&
                                                        u.province_id == province.id &&
                                                        u.city_id == city.id &&
                                                        u.district_id == district.id &&
                                                        u.pos_details == this.request.Details
                                                    ).FirstOrDefault();
            if (upos == null)
            {
                upos = new t_user_pos()
                {
                    id = 0,
                    create_date = DateTime.Now,
                    update_date = DateTime.Now,
                    geohash_key = Geohash.Encode(this.request.Latitude, this.request.Longitude),
                    lat = this.request.Latitude,
                    lon = this.request.Longitude,
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

            return upos;
        }
    }
}
