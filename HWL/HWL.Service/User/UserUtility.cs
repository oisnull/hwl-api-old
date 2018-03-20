using HWL.Entity;
using HWL.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HWL.Service.User
{
    public class UserUtility
    {
        public static string GetShowName(string name, string symbol)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
            if (!string.IsNullOrEmpty(symbol))
            {
                return symbol;
            }
            return "匿名";
        }

        public static string GetShowName(string nameRemark, string name, string symbol)
        {
            if (!string.IsNullOrEmpty(nameRemark))
            {
                return nameRemark;
            }
            return GetShowName(name, symbol);
        }

        public static string BuildToken(int userId)
        {
            byte[] guid = Guid.NewGuid().ToByteArray();
            string key = Convert.ToString(userId, 2);

            string token = key + "-" + String.Join("-", guid);

            return CommonCs.GetMd5Str32(token);
        }

        ///// <summary>
        ///// 加密用户token模型信息,作为请求的唯一凭证
        ///// </summary>
        ///// <returns></returns>
        //public static string BuildToken(UserTokenInfo tokenInfo)
        //{
        //    if (tokenInfo == null || tokenInfo.UserId <= 0) throw new Exception("用户凭证生成错误");
        //    if (tokenInfo.Expire == null || tokenInfo.Expire <= DateTime.Now) throw new Exception("用户凭证过期设置错误");

        //    return JsonConvert.SerializeObject(tokenInfo);
        //}

        ///// <summary>
        ///// 用户访问凭证解析处理
        ///// </summary>
        ///// <param name="tokenInfoJsonStr"></param>
        ///// <returns></returns>
        //public static UserTokenInfo ParserToken(string tokenInfoJsonStr)
        //{
        //    if (string.IsNullOrEmpty(tokenInfoJsonStr)) throw new Exception("用户访问凭证是空的");

        //    try
        //    {
        //        return JsonConvert.DeserializeObject<UserTokenInfo>(tokenInfoJsonStr);
        //    }
        //    catch
        //    {
        //        throw new Exception("用户访问凭证解析错误");
        //    }
        //}

        /// <summary>
        /// 向数据库中添加验证码
        /// </summary>
        public static int AddCode(CodeType codeType, string code, string remark = "", string userAccount = "", int userId = 0)
        {
            using (HWLEntities db = new HWLEntities())
            {
                DateTime currTime = DateTime.Now;
                t_user_code model = new t_user_code()
                {
                    id = 0,
                    code = code,
                    code_type = codeType,
                    create_date = currTime,
                    expire_time = currTime.AddSeconds(ConfigManager.UserCodeExpireSecond),
                    remark = remark,
                    user_id = userId,
                    user_account = userAccount,
                };
                db.t_user_code.Add(model);
                db.SaveChanges();
                return model.id;
            }
        }

        /// <summary>
        /// 获取名称备注中的第一个字母,如果第一个不是字母则返回#
        /// </summary>
        /// <param name="remark"></param>
        /// <returns></returns>
        public static string GetRemarkFirstSpell(string remark)
        {
            if (string.IsNullOrEmpty(remark)) return "#";
            string fristSpellStr = SpellString.GetFirstSpell(remark);

            Regex reg = new Regex("^[A-Za-z]+$");
            if (reg.IsMatch(fristSpellStr))
            {
                return fristSpellStr.ToUpper();
            }
            else
            {
                return "#";
            }
        }
    }
}
