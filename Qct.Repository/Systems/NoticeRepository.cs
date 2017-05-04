using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Infrastructure.Data.Extensions;
using Qct.Objects.ValueObjects;
using System.Collections.Specialized;

namespace Qct.Repository
{
    public class NoticeRepository : BaseEFRepository<Notice>, INoticeRepository
    {
        public OperateResult ChangeState(long[] ids, int type)
        {
            throw new NotImplementedException();
        }

        public PageInformaction<Notice> FindPageList(NameValueCollection nvl)
        {
            throw new NotImplementedException();
        }

        public List<Notice> GetNewestNotice(int takeNum,string currentStoreId)
        {
            var query = GetReadOnlyEntities().Where(o=>o.State==1);
            if(!string.IsNullOrWhiteSpace(currentStoreId))
            {
                query = query.Where(o => ("," + o.StoreId + ",").Contains("," + currentStoreId + ","));
            }
            return query.OrderByDescending(n => n.CreateDT).Take(takeNum).ToList();
        }

        public object GetNoticeList(string userCode)
        {
            throw new NotImplementedException();
        }

        public object GetNoticeNum(string userCode)
        {
            throw new NotImplementedException();
        }

        public OperateResult SaveOrUpdate(Notice obj)
        {
            throw new NotImplementedException();
        }
    }
}
