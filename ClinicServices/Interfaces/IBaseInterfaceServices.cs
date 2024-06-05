using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicServices.Interfaces
{
    public interface IBaseInterfaceServices<T>
    {
        public Task<T> Get();
        public Task<T> Get(int id);
        public Task<T> Get(Func<T,bool> predicate);
        public Task<T> Add(T item);
        public Task<T> Update(T item);
        public Task<T> Remove(T item);
    }
}
