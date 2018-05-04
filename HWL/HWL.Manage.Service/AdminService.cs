using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HWL.Manage.Service
{
    public class AdminService : BaseService
    {
        /// <summary>
        /// 系统管理员登录
        /// </summary>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Admin Login(string name, string password)
        {
            Admin model = new Admin();

            var adminEntity = dbContext.t_admin.SingleOrDefault(m => m.login_name == name);
            if (adminEntity == null)
            {
                model.LoginStatus = AdminLoginStatus.NoExists;
                return model;
            }
            if (adminEntity.login_pwd != password)
            {
                model.LoginStatus = AdminLoginStatus.PasswordError;
                return model;
            }

            model.LoginStatus = AdminLoginStatus.Success;
            model.LoginName = adminEntity.login_name;
            model.CreateTime = adminEntity.create_date;
            model.RealName = adminEntity.real_name;

            return model;

        }
    }
}
