using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamllHax.MapleSyrup.Providers.Dumper.Nodes
{
    public class WzDirectory : WzNode
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

        public int? GetIntValueFromChild(string name)
        {
            var child = Children.SingleOrDefault(x => x.Name == name);
            if (child == null)
            {
                return null;
            }
            var intNode = child as WzIntValue;
            if (intNode != null)
            {
                return intNode.Value;
            }
            var stringNode = child as WzStringValue;
            if (stringNode == null)
            {
                throw new Exception($"Node of invalid type, expected WzInt or WzString, got {child.GetType().Name}");
            }
            return Convert.ToInt32(stringNode.Value);
        }
    }
}
