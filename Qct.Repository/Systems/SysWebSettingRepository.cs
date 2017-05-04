using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Qct.Objects.Entities;
using Qct.Repository;
using Qct.Objects.ValueObjects;
using Qct.Infrastructure.Extensions;
using Qct.IRepository;

namespace Qct.Repository
{
    public class SysWebSettingRepository : BaseEFRepository<SysWebSetting>, ISysWebSettingRepository
    {
        public void SaveWebSetting(SysWebSetting model)
        {
            if (model.Id == 0)
                CreateWithSaveChanges(model);
            else
            {
                var obj = Get(model.Id);
                model.ToCopyProperty(obj);
                SaveChanges();
            }
        }
        /// <summary>
        /// 获取基本配置信息列表
        /// </summary>
        /// <returns></returns>
        public SysWebSetting GetWebSetting()
        {
            return GetReadOnlyEntities().FirstOrDefault();
        }
        /// <summary>
        /// 获取logo
        /// </summary>
        /// <returns></returns>
        public string GetLogo()
        {
            SysWebSetting s = GetWebSetting();
            if (s != null)
            {
                if (string.IsNullOrEmpty(s.LoginLogo))
                {
                    return "";
                }
                return "/SysImg/" + CompanyId + "/" + s.LoginLogo;
            }
            else
            {
                return "";
            }
        }
    }
}
