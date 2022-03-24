using ApiEmpleadosOAuth.Models;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;

        public EmpleadosController(RepositoryEmpleados repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<List<Empleado>> GetEmpleados()
        {
            return this.repo.GetEmpleados();
        }

        [HttpGet("{id}")]
        public ActionResult<Empleado> FindEmpleado(int id)
        {
            return this.repo.FindEmpleado(id);
        }
    }
}
