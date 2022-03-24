using ApiEmpleadosOAuth.Helpers;
using ApiEmpleadosOAuth.Models;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private RepositoryEmpleados repo;
        private HelperOAuthToken helper;

        public AuthController(RepositoryEmpleados repo
            , HelperOAuthToken helper)
        {
            this.repo = repo;
            this.helper = helper;
        }

        //NECESITAMOS UN METODO PARA VALIDAR EL USUARIO/PASSWORD 
        //Y QUE RECIBIRA UN LOGINMODEL.
        //LOS ENDPOINTS DE VALIDACION SON POST
        [HttpPost]
        [Route("[action]")]
        public ActionResult Login(LoginModel model)
        {
            Empleado empleado =
                this.repo.ExisteEmpleado(model.UserName
                , int.Parse(model.Password));
            if (empleado == null)
            {
                return Unauthorized();
            }
            else
            {
                //UN TOKEN CONTIENE UNAS CREDENCIALES
                SigningCredentials credentials =
                    new SigningCredentials(this.helper.GetKeyToken()
                    , SecurityAlgorithms.HmacSha256);
                //ES EL MOMENTO DE GENERAR EL TOKEN
                //EL TOKEN ESTARA COMPUESTO POR ISSUER, AUDIENCE, CREDENTIALS
                //TIME
                JwtSecurityToken token =
                    new JwtSecurityToken(
                        issuer: this.helper.Issuer,
                        audience: this.helper.Audience,
                        signingCredentials: credentials,
                        expires: DateTime.UtcNow.AddMinutes(30),
                        notBefore: DateTime.UtcNow
                        );
                //DEVOLVEMOS UNA RESPUESTA CORRECTA CON EL TOKEN
                return Ok(
                    new
                    {
                        response =
                        new JwtSecurityTokenHandler().WriteToken(token)
                    });
            }
        }
    }
}
