using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Interface
{
    public interface ICacheService
    {
        Task<T> GetDataAsync<T>(string key);

        Task SetDataAsync<T>(string key, T value);

        Task RemoveDataAsync(string key);
    }
}
