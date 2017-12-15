using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using api_core_ef_sqlinux_docker.Models;
using Microsoft.Extensions.Logging;

namespace api_core_ef_sqlinux_docker.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private readonly TodoDbContext todoContext;
        private readonly ILogger _logger;
        public TodoController(TodoDbContext todoContext, ILogger<TodoController> logger)
        {
            this.todoContext = todoContext;
            //_logger = logger.CreateLogger("TodoApi.Controllers.TodoController");
            _logger = logger;
        }

        // GET api/todo
        [HttpGet]
        public IEnumerable<Todo> Get()
        {
            _logger.LogInformation("getting todos....");
            return todoContext.Todos;
        }

        // GET api/todo/name
        [HttpGet("{id}")]
        public string Get(string name)
        {
            return name;
        }

        // POST api/todo
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
