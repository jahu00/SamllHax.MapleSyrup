using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Data
{
    public class WzDirectory: WzNode
    {
        public List<WzNode> Children { get; set; } = new List<WzNode>();
        public T GetSingleChild<T>(string name) where T : class
        {
            var child = Children.Single(x => x.Name == name);
            var typedChild = child as T;
            if (typedChild == null)
            {
                throw new Exception($"Child {name} is not of type {typeof(T).Name}, but {child.GetType().Name}");
            }
            return typedChild;
        }

        public T GetSingleOrDefaultChild<T>(string name) where T : class
        {
            var child = Children.SingleOrDefault(x => x.Name == name);
            if (child == null)
            {
                return null;
            }
            var typedChild = child as T;
            if (typedChild == null)
            {
                throw new Exception($"Child {name} is not of type {typeof(T).Name}, but {child.GetType().Name}");
            }
            return typedChild;
        }
    }
}
