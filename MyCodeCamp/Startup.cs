using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;

namespace MyCodeCamp
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

       

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.RegisterServices(Configuration);


            // var config = new WeblogConfiguration();
            //Configuration.Bind("Weblog", config);      //  <--- This
            //services.AddSingleton(config);

            //services.AddSingleton<WeblogConfiguration>(conf =>
            //{
            //    configuration.Bind()
            //});

            //services.AddSingleton(cw =>
            //{
            //    var config = new WeblogConfiguration();
            //    Configuration.Bind("Weblog", config);
            //    return config;
            //});



            services.AddAutoMapper();

            services.AddCors();

            services.AddMvc( setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
                setupAction.OutputFormatters.Add(new XmlDataContractSerializerOutputFormatter());
                //setupAction.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            //services.AddMvc()
             //       .AddXmlSerializerFormatters();
                    //.AddJsonOptions(options =>
                   // {
                  //      options.SerializerSettings.Formatting = Formatting.Indented;
                  //  });

            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My Code Camp",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = "https://twitter.com/spboyer"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });


            services.AddTokenAuthentication();



            //var appSettings = services.BuildServiceProvider().GetService<IAppSettings>();
           // var tokenSttings = services.BuildServiceProvider().GetService<ITokenSettings>();
         
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //}).AddJwtBearer(options =>
            //{
            //    options.RequireHttpsMetadata = false;
            //    options.SaveToken = true;
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSttings.SecretKey)),
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true
            //    };

            //    options.Events = new JwtBearerEvents
            //    {
            //        OnAuthenticationFailed = context =>
            //        {
            //            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            //            {
            //                context.Response.Headers.Add("Token-Expired", "true");
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };
            //});

        }

        //public void ConfigureProductionServices(IServiceCollection services)
        //{
        //    services.AddDistributedSqlServerCache(options =>
        //    {
        //        options.ConnectionString =
        //            @"Data Source=(localdb)\v11.0;Initial Catalog=DistCache;" +
        //            @"Integrated Security=True;";
        //        options.SchemaName = "dbo";
        //        options.TableName = "TestCache";
        //    });
        //}

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
           else{
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            var logger = loggerFactory.CreateLogger("Global exception logger");
                            logger.LogError(StatusCodes.Status500InternalServerError, 
                                            exceptionHandlerFeature.Error, 
                                            exceptionHandlerFeature.Error.Message);
                        }
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("An unexpected fault happened.  Please, try again later.");
                    });
                    
                });
            }

            app.UseCors(builder => builder.AllowAnyOrigin());

            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.InjectStylesheet("/swagger/ui/custom.css");
            });

           
            //app.UseJwtBearerAuthentication(new JwtBearerOptions()
            //{
            //    Auto
            //});

            app.UseAuthentication();
            app.UseMvc();

        }
    }
}
