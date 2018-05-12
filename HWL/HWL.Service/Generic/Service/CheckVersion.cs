using GMSF;
using HWL.Entity;
using HWL.Service.Generic.Body;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Service.Generic.Service
{
    public class CheckVersion : ServiceHandler<CheckVersionRequestBody, CheckVersionResponseBody>
    {
        public CheckVersion(CheckVersionRequestBody request) : base(request) { }

        protected override void ValidateRequestParams()
        {
            if (this.request.UserId <= 0)
            {
                throw new Exception("用户信息错误");
            }
            if (string.IsNullOrEmpty(this.request.OldVersion))
            {
                throw new Exception("版本号错误");
            }
        }

        public override CheckVersionResponseBody ExecuteCore()
        {
            CheckVersionResponseBody res = new CheckVersionResponseBody();
            using (HWLEntities db = new HWLEntities())
            {
                var version = db.t_app_version.OrderByDescending(v => v.publish_time).FirstOrDefault();
                if (version == null)
                {
                    res.IsNewVersion = false;
                    return res;
                }
                if (GenericUtility.CompareVersion(this.request.OldVersion, version.app_version) == -1)
                {
                    res.IsNewVersion = true;
                    res.NewVersion = version.app_version;
                    res.DownLoadUrl = version.download_url;
                }
                else
                {
                    res.IsNewVersion = false;
                }
            }
            return res;
        }
    }
}
