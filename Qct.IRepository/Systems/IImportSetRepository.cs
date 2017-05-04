using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Qct.Objects.ValueObjects;
using System.Web;

namespace Qct.IRepository
{
    public interface IImportSetRepository:Qct.Infrastructure.Data.IEFRepository<ImportSet>
    {
        ImportSet GetByName(string tableName);
    }
}
