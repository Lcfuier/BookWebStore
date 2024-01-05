using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Query
{
    public static class QueryExtensions
    {
        public static IQueryable<T> PageBy<T>(this IQueryable<T> items,
            int pageNumber, int pageSize)
        {
            return items
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
        }
    }
}
