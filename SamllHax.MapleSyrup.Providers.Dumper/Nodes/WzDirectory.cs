using SamllHax.MapleSyrup.Interfaces.Data;
using SamllHax.MapleSyrup.Providers.Dumper.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

        public List<IVectorCollection> GetVectorCollections(string name)
        {
            var matchingChildren = Children.Where(x => x.Name == name).ToArray();
            if (matchingChildren.Count() == 0)
            {
                return null;
            }
            if (matchingChildren.All(x => x is WzVector))
            {
                return new List<IVectorCollection>() { new WzVectorCollection(this, matchingChildren.Cast<WzVector>().ToList()) };
            }
            if (matchingChildren.Count() > 1)
            {
                throw new Exception($"Expected no more than child with name {name}");
            }
            var child = matchingChildren.Single();
            var extendedChild = child as WzExtended;
            if (extendedChild != null)
            {
                return new List<IVectorCollection>() { extendedChild.ToVectorCollection() };
            }
            var directoryChild = child as WzDirectory;
            if (directoryChild == null)
            {
                throw new Exception($"Unupported VectorCollection of type {directoryChild.GetType().Name}");
            }
            if (directoryChild.Children.All(x => x is WzVector))
            {
                return new List<IVectorCollection>() { new WzVectorCollection(this, directoryChild.Children.Cast<WzVector>().ToList()) };
            }
            if (directoryChild.Children.All(x => x is WzExtended))
            {
                return directoryChild.Children.Cast<WzExtended>().Select(x => x.ToVectorCollection()).ToList<IVectorCollection>();
            }
            throw new Exception($"Unsupported VectorCollection");
        }
    }
}
