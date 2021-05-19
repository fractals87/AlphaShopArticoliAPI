using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlphaShopArticoliAPI.Security;
using AlphaShopArticoliAPI.Services;
using AlphaShopGestUserAPI.Helper;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace AlphaShopArticoliAPI
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
            services.AddCors();

            //USATO PER CORREGGERE
            //System.Text.Json.JsonException: A possible object cycle was detected which is not supported. This can either be due to a cycle or if the object depth is larger than the maximum allowed depth of 32.
            services.AddControllers()
                    .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var connectionString = Configuration["connectionStrings:alphashopDbConString"];
            services.AddDbContext<AlphaShopDbContext>(c => c.UseSqlServer(connectionString));

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            //Otteniamo la parola chiave
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);


            //services.AddAuthentication("BasicAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => 
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddScoped<IArticoliRepository, ArticoliRepository>();
            //services.AddScoped<IUserService, UserService>(); Non serve più con autenticazione JWT

            //USATO PER disabilitare il return dell'errore delle dataannotation
            /*
             * {
                    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "title": "One or more validation errors occurred.",
                    "status": 400,
                    "traceId": "|982ea20f-413a1e904a80716d.",
                    "errors": {
                        "Descrizione": [
                            "La Descrizione deve avere almeno 5 caratteri"
                        ]
                    }
                }
             */
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            //services.AddDbContext<AlphaShopDbContext>(options =>
            //    options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            //TODO: CONFIGURAZIONE SOLO PER SVILUPPO
        //    services.AddCors(options =>
        //    {
        //        options.AddDefaultPolicy(builder =>
        //            builder.SetIsOriginAllowed(_ => true)
        //                   .AllowAnyMethod()
        //                   .AllowAnyHeader()
        //                   .AllowCredentials());
        //    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //TODO: CONFIGURAZIONE SOLO PER SVILUPPO

            //app.UseCors(x => x
            //.AllowAnyOrigin()
            //.AllowAnyMethod()
            //.AllowAnyHeader()
            //.AllowCredentials()
            //);
            app.UseCors(
                x => x
                .AllowAnyOrigin()
                .WithMethods("POST", "PUT", "DELETE", "GET")
                .AllowAnyHeader()
            );

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
