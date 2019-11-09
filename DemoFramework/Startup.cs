using System;
using System.Linq;
using DemoFrame_Basic;
using DemoFrame_Basic.Dependency;
using DemoFrame_CoreMvc;
using DemoFrame_CoreMvc.Filters;
using DemoFrame_Dao;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog.Web;

namespace DemoFramework_MainWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            env.ConfigureNLog("NLog.config");
            DemoWeb.Configuration = configuration;
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.MaxModelValidationErrors = 5;//验证错误最大个数
                options.AllowValidatingTopLevelNodes = true;//是否允许验证顶级节点  接口方法参数
                options.Filters.Add(new ExceptionFilter());//添加异常处理过滤器
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            services.AddDbContext<DemoDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("SqlServerConnection")));
            services.AddScoped<DemoDbContext>();
            return IocManager.Instance.Initialize(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IMemoryCache memoryCache)
        {
            DemoWeb.IocManager = app.ApplicationServices.GetService<IIocManager>();
            DemoWeb.Environment = env;
            try//注意这里在本地开发允许时会重置数据库，并清空所有数据，如不需要请注释   在不存在数据库的时候请将这个地方的注释打开
            {
                if (env.IsDevelopment())
                {
                    using (var servicescope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                        .CreateScope())
                    {
                        //var dbcontent = servicescope.serviceprovider.getservice<demodbcontext>();
                        //checkmigrations(dbcontent);
                        var database = servicescope.ServiceProvider.GetService<DemoDbContext>().Database;
                        database.EnsureDeleted();
                        database.EnsureCreated();
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Logger.Error(ex, "Failed to migrate or seed database");
            }
            DemoWeb.Configure(app.ApplicationServices.GetRequiredService<IHttpContextAccessor>());
            DemoWeb.MemoryCache = memoryCache;
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());//允许跨域
            app.UseHttpsRedirection();
            app.UseMvc(routes => { routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}"); });
            //app.UseMvc();
        }

        /// <summary>
        /// 检查迁移
        /// </summary>
        /// <param name="db"></param>
        private void CheckMigrations(DemoDbContext db)
        {
            //var a = db.Database.GetAppliedMigrations().ToArray();
            //var c = db.Database.GetPendingMigrations().ToArray();
            //var b = db.Database.GetMigrations();
            //判断是否有待迁移
            if (db.Database.GetPendingMigrations().Any())
            {
                //执行迁移
                db.Database.Migrate();
            }
        }
    }
}
