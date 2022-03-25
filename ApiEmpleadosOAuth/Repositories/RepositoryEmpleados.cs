using ApiEmpleadosOAuth.Data;
using ApiEmpleadosOAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;

        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados()
        {
            return this.context.Empleados.ToList();
        }

        public Empleado FindEmpleado(int idempleado)
        {
            return this.context.Empleados.SingleOrDefault(x => x.IdEmpleado == idempleado);
        }

        public Empleado ExisteEmpleado(string apellido, int empno)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.Apellido == apellido
                           && datos.IdEmpleado == empno
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return consulta.First();
            }
        }

        //METODO PARA DEVOLVER LOS SUBORDINADOS DE UN EMPLEADO
        public List<Empleado> GetSubordinados(int idDirector)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.Director == idDirector
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return consulta.ToList();
            }
        }

        //METODO PARA DEVOLVER LOS EMPLEADOS DE UN DEPARTAMENTO
        public List<Empleado> GetCompisCurro(int idDepartamento)
        {
            var consulta = from datos in this.context.Empleados
                           where datos.IdDepartamento == idDepartamento
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                return consulta.ToList();
            }
        }
    }
}
