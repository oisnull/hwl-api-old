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
    public class SearchUser : GMSF.ServiceHandler<SearchUserRequestBody, SearchUserResponseBody>
    {
        public SearchUser(SearchUserRequestBody request) : base(request)
        {
        }

        protected override void ValidateRequestParams()
        {
            base.ValidateRequestParams();

            if (this.request.UserId <= 0)
            {
                throw new ArgumentNullException("UserId");
            }
            if (string.IsNullOrEmpty(this.request.UserKey))
            {
                throw new ArgumentNullException("UserKey");
            }
        }

        public override SearchUserResponseBody ExecuteCore()
        {
            SearchUserResponseBody res = new SearchUserResponseBody();

            using (HWLEntities db = new HWLEntities())
            {
                var friendIds = db.t_user_friend.Where(u => u.user_id == this.request.UserId).Select(u => u.friend_user_id).ToList();
                if (friendIds == null)
                {
                    friendIds = new List<int>();
                }
                friendIds.Add(this.request.UserId);

                res.UserInfos = db.t_user.Where(u => u.symbol.Contains(this.request.UserKey) && !friendIds.Contains(u.id)).Select(u => new UserSearchInfo()
                {
                    HeadImage = u.head_image,
                    Id = u.id,
                    Symbol = u.symbol,
                    ShowName = u.name,
                }).ToList();

            }

            return res;
        }
    }
}
