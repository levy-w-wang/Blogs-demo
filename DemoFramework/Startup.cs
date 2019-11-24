using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DemoFrame_Basic;
using DemoFrame_Basic.Dependency;
using DemoFrame_CoreMvc;
using DemoFrame_CoreMvc.Filters;
using DemoFrame_Dao;
using DemoFramework_MainWeb.Swagger;
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
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

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

            #region Swagger

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "API Doc",
                    Description = "作者:Levy_w_Wang",
                    //服务条款
                    TermsOfService = "None",
                    //作者信息
                    Contact = new Contact
                    {
                        Name = "levy",
                        Email = "levy_w_wang@qq.com",
                        Url = "https://www.cnblogs.com/levywang"
                    },
                    //许可证
                    License = new License
                    {
                        Name = "tim",
                        Url = "https://www.cnblogs.com/levywang"
                    }
                });

                #region XmlComments

                var basePath1 = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录(平台)影响，建议采用此方法获取路径）
                //获取目录下的XML文件 显示注释等信息
                var xmlComments = Directory.GetFiles(basePath1, "*.xml", SearchOption.AllDirectories).ToList();

                foreach (var xmlComment in xmlComments)
                {
                    options.IncludeXmlComments(xmlComment);
                }
                #endregion

                options.DocInclusionPredicate((docName, description) => true);

                options.OperationFilter<SwaggerParameter>();

                //options.AddSecurityDefinition("token", new ApiKeyScheme
                //{
                //    Description = "token format : {token}",//参数描述
                //    Name = "token",//名字
                //    In = "header",//对应位置
                //    Type = "apiKey"//类型描述
                //});
                //options.AddSecurityDefinition("sid", new ApiKeyScheme
                //{
                //    Description = "sid format : {sid}",//参数描述
                //    Name = "sid",//名字
                //    In = "header",//对应位置
                //    Type = "apiKey"//类型描述
                //});
                ////添加Jwt验证设置 设置为全局的，不然在代码中取不到
                //options.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {
                //    { "token", Enumerable.Empty<string>() },
                //    { "sid", Enumerable.Empty<string>() },
                //});

                options.IgnoreObsoleteProperties();//忽略 有Obsolete 属性的方法
                options.IgnoreObsoleteActions();
                options.DescribeAllEnumsAsStrings();
            });
            #endregion

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

            // Fetch errorInternal Server Error v1/swagger.json 错误  有方法未指明请求方式  GET POST
            #region Swagger

            app.UseSwagger(c => { c.RouteTemplate = "apidoc/{documentName}/swagger.json"; });
            app.UseSwaggerUI(c =>
            {
                c.RoutePrefix = "apidoc";
                c.SwaggerEndpoint("v1/swagger.json", "ContentCenter API V1");
                c.DocExpansion(DocExpansion.None);//默认文档展开方式
            });

            #endregion

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
