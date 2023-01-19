using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup
{
    public class CachedResourceWrapper: IDisposable
    {
        public List<object> Owners { get; } = new List<object>();
        public object Resource { get; }
        public bool IsAbandoned => Owners.Count == 0;

        public CachedResourceWrapper(object resource, object owner)
        {
            Resource = resource;
            Owners.Add(owner);
        }

        public T Cast<T>()
        {
            return (T)Resource;
        }

        public void Dispose()
        {
            var disposableResource = Resource as IDisposable;
            if (disposableResource == null)
            {
                return;
            }
            disposableResource?.Dispose();
        }
    }
}
