using ApiEmpleadosOAuth.Data;
using ApiEmpleadosOAuth.Helpers;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string azureSql =
                this.Configuration.GetConnectionString("azureSql");
            services.AddTransient<RepositoryEmpleados>();
            services.AddDbContext<EmpleadosContext>
                (options => options.UseSqlServer(azureSql));
            //HABILITAMOS LA SEGURIDAD JWT OAUTH
            HelperOAuthToken helper = new HelperOAuthToken(this.Configuration);
            services.AddAuthentication(helper.GetAuthenticationOptions())
                .AddJwtBearer(helper.GetJwtOptions());
            //VAMOS A PONER EN LA INYECCION EL HELPER
            //PORQUE LO UTILIZAREMOS DENTRO DE EL CONTROLLER DE AUTORIZACION
            services.AddTransient<HelperOAuthToken>
                (x => helper);
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { 
                    Title = "Api Empleados OAuth", 
                    Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiEmpleadosOAuth v1");
                c.RoutePrefix = "";
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
