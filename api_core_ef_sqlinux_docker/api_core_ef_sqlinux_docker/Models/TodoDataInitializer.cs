using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api_core_ef_sqlinux_docker.Models
{
    public static class TodoDataInitializer
    {
        public static void Initializer(TodoDbContext context)
        {
            context.Database.EnsureCreated();
            foreach (var iter in Enumerable.Range(0, 10))
            {
                var todo = new Todo() { Description = $"Todo#{iter}", CreatedDate = DateTime.UtcNow };
                context.Todos.Add(todo);
            }
            context.SaveChanges();
        }
    }
}
