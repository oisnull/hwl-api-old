using HWL.Entity;
using HWL.Entity.Extends;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HWL.Service.Generic
{
    public class AreaUtility
    {
        /// <summary>
        /// 获取所有省,市,区数据列表
        /// </summary>
        public static List<AreaModel> GetProCityDistList()
        {
            List<AreaModel> list = new List<AreaModel>();
            using (HWLEntities db = new HWLEntities())
            {
                var proList = db.t_province.ToList();

                var cityList = db.t_city.ToList();

                var distList = db.t_district.ToList();

                if (proList != null && proList.Count > 0)
                {
                    proList.ForEach(f =>
                    {
                        AreaModel pro = new AreaModel()
                        {
                            text = f.name,
                            value = f.id,
                            children = new List<AreaModel>(),
                        };

                        #region city
                        var cities = cityList.Where(c => c.province_id == f.id).ToList();
                        if (cities != null && cities.Count > 0)
                        {
                            cities.ForEach(fc =>
                            {
                                AreaModel city = new AreaModel()
                                {
                                    text = fc.name,
                                    value = fc.id,
                                    children = distList.Where(d => d.city_id == fc.id).Select(d => new AreaModel() { text = d.name, value = d.id }).ToList(),
                                };
                                pro.children.Add(city);
                            });
                        }
                        #endregion

                        list.Add(pro);
                    });
                }
            }

            return list;
        }


    }
}
