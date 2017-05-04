using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Qct.Infrastructure.Data.EntityInterface
{
    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public static class EntityExtensions
    {
        public static string GetGuid(this IEntity<string> entity)
        {
            return Guid.NewGuid().ToString("N");
        }
    }
}
