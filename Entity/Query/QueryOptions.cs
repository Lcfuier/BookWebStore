using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Query
{
    public class QueryOptions <T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        private string[] includes =Array.Empty<string>();

        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(",");
        }
        public string[] getIncludes() => includes;

        public List<Expression<Func<T, bool>>> WhereClauses { get; set; } = null!;

        public Expression<Func<T, bool>> Where
        {
            set
            {
                // if WhereClauses is null then create a new one.
                WhereClauses ??= new List<Expression<Func<T, bool>>>();
                WhereClauses.Add(value);
            }
        }
        public Expression<Func<T, object>> OrderBy { get; set; } = null!;
        public string OrderByDirection { get; set; } = "asc"; // only if have orderby to invoke this.

        // flags.
        public bool HasPaging => PageNumber > 0 && PageSize > 0;
        public bool HasInclude => includes != Array.Empty<string>();
        public bool HasWhere => WhereClauses is not null;
        public bool HasOrderBy => OrderBy is not null;
    }
}
