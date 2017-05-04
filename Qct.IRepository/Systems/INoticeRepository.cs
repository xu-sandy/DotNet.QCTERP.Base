using Qct.Infrastructure.Data.Extensions;
using Qct.Objects.Entities;
using Qct.Objects.ValueObjects;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Qct.IRepository
{
    public interface INoticeRepository
    {
        OperateResult ChangeState(long[] ids, int type);
        PageInformaction<Notice> FindPageList(NameValueCollection nvl);
        List<Notice> GetNewestNotice(int takeNum, string currentStoreId);
        object GetNoticeList(string userCode);
        object GetNoticeNum(string userCode);
        OperateResult SaveOrUpdate(Notice obj);
    }
}
