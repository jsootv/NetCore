using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.Web.V31
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
            // .Net Core 2.1의 AddMvc()에서 다음과 같이 메서드명이 변경됨. 
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            // 아래의 app.UseEndpoints()메서드를 라우팅과 연결하기 위해 사용됨.
            app.UseRouting();

            // 승인권한을 사용하기 위해 추가됨.
            app.UseAuthorization();

            // .Net Core 2.1의 UseMvc()에서 다음과 같이 메서드명이 변경됨. 
            app.UseEndpoints(endpoints =>
            {
                // .Net Core 2.1의 UseMvc()에서 다음과 같이 메서드명이 변경됨.
                endpoints.MapControllerRoute(
                    name: "default",
                    // .Net Core 2.1의 template에서 다음과 같이 파라미터명이 변경됨.
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
