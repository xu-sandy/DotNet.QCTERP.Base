using Qct.Infrastructure.Extensions;
using Qct.IRepository;
using Qct.Objects.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Repository.Systems
{
    public class ImportSetRepository : BaseEFRepository<ImportSet>, IImportSetRepository
    {
        public ImportSet GetByName(string tableName)
        {
            return GetEntities().FirstOrDefault(o => o.TableName == tableName);
        }
        public override dynamic AddOrUpdate(params ImportSet[] objs)
        {
            foreach (var obj in objs)
            {
                if (obj.Id == 0)
                {
                    if (!IsExists(o => o.TableName == obj.TableName))
                    {
                        Create(obj);
                    }
                }
                else
                {
                    var source = Get(obj.Id);
                    obj.ToCopyProperty(source);
                }
            }
            SaveChanges();
            return true;
        }
    }
}
