using Castle.Core.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ProjetoFinal_Livros
{
    public class Startup
    {
        private readonly Castle.Core.Configuration.IConfiguration _configuration;

        public Startup(Castle.Core.Configuration.IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public void ConfigureServices(IServiceCollection services)
        //{
        //    string connectionString = Configuration.GetConnectionString("NomeDaChaveDaConexao");

        //    services.AddDbContext<Context>(options =>
        //        options.UseSqlServer(connectionString));


        //    services.AddControllersWithViews();
        //}

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Configurações de produção, como tratamento de erros personalizados, etc.
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
