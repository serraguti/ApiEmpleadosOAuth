using ApiEmpleadosOAuth.Models;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        [Route("[action]")]
        public ActionResult<Empleado> PerfilEmpleado()
        {
            //AQUI HEMOS RECIBIDO EL TOKEN
            //CUANDO RECIBIMOS EL TOKEN, SE MONTA EL SERVICIO Y 
            //ESTAMOS DENTRO DE USER
            //ESTAMOS RECUPERANDO LOS CLAIMS DEL TOKEN
            List<Claim> claims = HttpContext.User.Claims.ToList();
            //RECUPERAMOS LA KEY UserData QUE ES LA INFORMACION
            //DEL EMPLEADO EN FORMATO JSON
            string jsonEmpleado =
                claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(jsonEmpleado);
            return empleado;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<List<Empleado>> Subordinados()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string json = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(json);
            List<Empleado> subordinados = this.repo.GetSubordinados(empleado.IdEmpleado);
            return subordinados;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public ActionResult<List<Empleado>> Compis()
        {
            List<Claim> claims = HttpContext.User.Claims.ToList();
            string json = claims.SingleOrDefault(z => z.Type == "UserData").Value;
            Empleado empleado = JsonConvert.DeserializeObject<Empleado>(json);
            List<Empleado> empleados = this.repo.GetCompisCurro(empleado.IdDepartamento);
            return empleados;
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
