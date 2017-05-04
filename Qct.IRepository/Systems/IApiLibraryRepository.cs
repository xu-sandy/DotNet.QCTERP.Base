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
    public interface IApiLibraryRepository : IEFRepository<ApiLibrary>
    {
        PageInformaction FindPageList(NameValueCollection nvl);
        void MoveItem(int mode, int id);
        List<ApiLibrary> GetList(bool all=false);
        void SetState(string ids, short state);
    }
}
