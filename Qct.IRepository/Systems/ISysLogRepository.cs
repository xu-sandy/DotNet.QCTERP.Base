using Qct.Infrastructure.Data;
using Qct.Infrastructure.Data.Extensions;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.IRepository
{
    public interface ISysLogRepository: IEFRepository<SysLog>
    {
        PageInformaction FindPageList(NameValueCollection nvl);
        void DeleteIds(int[] ids);
        void DeleteAll();
    }
}
