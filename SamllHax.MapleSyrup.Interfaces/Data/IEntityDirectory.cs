using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SamllHax.MapleSyrup.Interfaces.Data
{
    public interface IEntityDirectory<TEntity> : IEntity where TEntity : IEntity
    {
        IDictionary<string, TEntity> Entities { get; }
        IDictionary<string, IEntityDirectory<TEntity>> Directories { get; }
    }

    public static class EntityDirectoryExtensions
    {
        public static T GetEntityByPath<T>(this IEntityDirectory<T> entityDirectory, params string[] path) where T : IEntity
        {
            var currentDirectory = entityDirectory;
            var entityName = path.Last();
            foreach (var currentName in path.Take(path.Length - 1))
            {
                currentDirectory = currentDirectory.Directories[currentName];
            }
            return currentDirectory.Entities[entityName];
        }

        public static IEntity GetByPath<T>(this IEntityDirectory<T> entityDirectory, params string[] path) where T : IEntity
        {
            var currentDirectory = entityDirectory;
            var entityName = path.Last();
            foreach (var currentName in path.Take(path.Length - 1))
            {
                currentDirectory = currentDirectory.Directories[currentName];
            }
            if (currentDirectory.Directories.TryGetValue(entityName, out var directory))
            {
                return directory;
            }
            return currentDirectory.Entities[entityName];
        }
    }
}
