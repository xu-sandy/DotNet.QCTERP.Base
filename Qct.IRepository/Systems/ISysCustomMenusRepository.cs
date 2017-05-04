using Qct.Infrastructure.Data;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.IRepository
{
    public interface ISysCustomMenusRepository: IEFRepository<SysCustomMenus>
    {
        SysCustomMenus GetbyObjid(int id);
    }
}
