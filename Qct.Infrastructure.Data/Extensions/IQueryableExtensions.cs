using System;
using System.Collections.Generic;
using System.Linq;

namespace Qct.Infrastructure.Data.Extensions
{
    /// <summary>
    /// 分页扩展
    /// </summary>
    public static class IQueryableExtensions
    {
        public static IEnumerable<TEntity> GetPage<TEntity>(this IQueryable<TEntity> entites, int pageNum = 1, int pageSize = 50)
        {
            return entites.GetPageWithQueryable(pageNum, pageSize); 
        }
        public static IQueryable<TEntity> GetPageWithQueryable<TEntity>(this IQueryable<TEntity> entites, int pageNum = 1, int pageSize = 50)
        {
            return entites.Skip((pageNum - 1) * pageSize).Take(pageSize);
        }

        public static PageInformaction<TEntity> GetPageWithInformaction<TEntity>(this IQueryable<TEntity> entites, int pageNum = 1, int pageSize = 50)
        {
            var info = new PageInformaction<TEntity>();
            info.CollectinSize = entites.Count();
            float count = info.CollectinSize / (float)pageSize;
            info.PageCount = (int)count;
            if (count > info.PageCount)
            {
                info.PageCount++;
            }
            info.PageSize = pageSize;
            info.PageNum = pageNum;
            info.Datas = entites.GetPage(pageNum, pageSize);
            return info;
        }
    }
    public class PageInformaction
    {
        public PageInformaction() { }
        public PageInformaction(int collectSize,int pageNum, int pageSize)
        {
            
        }
        /// <summary>
        /// 当前页号
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 页面项目数量
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 集合项目数量
        /// </summary>
        public int CollectinSize { get; set; }

        public IEnumerable<object> Datas { get; set; }
        /// <summary>
        /// 用于easyui页脚信息
        /// </summary>
        public IEnumerable<object> Footers { get; set; }
        /// <summary>
        /// 计算页数
        /// </summary>
        /// <returns></returns>
        public PageInformaction ToPageCount()
        {
            float count = CollectinSize / (float)PageSize;
            PageCount = (int)count;
            if (count > PageCount)
            {
                PageCount++;
            }
            return this;
        }
    }
    public class PageInformaction<TEntity> : PageInformaction
    {
        public new IEnumerable<TEntity> Datas
        {
            get
            {
                if (base.Datas == null)
                    return null;
                return base.Datas.Cast<TEntity>();
            }
            set
            {
                if (value == null)
                    base.Datas = null;
                else 
                    base.Datas = value.Cast<object>();
            }
        }
        /// <summary>
        /// 计算页数
        /// </summary>
        /// <returns></returns>
        public new PageInformaction<TEntity> ToPageCount()
        {
            float count = CollectinSize / (float)PageSize;
            PageCount = (int)count;
            if (count > PageCount)
            {
                PageCount++;
            }
            return this;
        }

    }
}
