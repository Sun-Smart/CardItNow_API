using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LoggerService;

using NLog;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using carditnow.Services;
using carditnow.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using nTireBO.Services;
using nTireBO.Models;
namespace CardItNow
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "http://108.60.219.44:63939/",
                        ValidAudience = "http://108.60.219.44:63939/",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey")) //Configuration["Jwt:Key"]
                    };
                });

            services.AddSingleton<ILoggerManager, LoggerManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IReportViewerService, ReportViewerService>();
            services.AddDbContext<ReportViewerContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddScoped<IavatarmasterService, avatarmasterService>();
            services.AddScoped<IboconfigvalueService, boconfigvalueService>();
            services.AddDbContext<boconfigvalueContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddScoped<IcarditchargesdiscountService, carditchargesdiscountService>();
            services.AddScoped<IcitymasterService, citymasterService>();
            services.AddScoped<IcustomerdetailService, customerdetailService>();
            services.AddScoped<IcustomermasterService, customermasterService>();
            services.AddScoped<IcustomerpaymodeService, customerpaymodeService>();
            services.AddScoped<IcustomerrecipientlinkService, customerrecipientlinkService>();
            services.AddScoped<IcustomersecurityquestionService, customersecurityquestionService>();
            services.AddScoped<IcustomersecurityquestionshistoryService, customersecurityquestionshistoryService>();
            services.AddScoped<IcustomertermsacceptanceService, customertermsacceptanceService>();
            services.AddScoped<IgeoaccessService, geoaccessService>();
            services.AddScoped<IgeographymasterService, geographymasterService>();
            services.AddScoped<IinitiatorrecipientmappingService, initiatorrecipientmappingService>();
            services.AddScoped<IinitiatorrecipientprivateService, initiatorrecipientprivateService>();
            services.AddScoped<ImasterdataService, masterdataService>();
            services.AddScoped<ImasterdatatypeService, masterdatatypeService>();
            services.AddScoped<ImenuaccessService, menuaccessService>();
            services.AddScoped<ImenumasterService, menumasterService>();
            services.AddScoped<IrecipientdiscountService, recipientdiscountService>();
            services.AddScoped<ItermsmasterService, termsmasterService>();
            services.AddScoped<ItransactiondetailService, transactiondetailService>();
            services.AddScoped<ItransactionitemdetailService, transactionitemdetailService>();
            services.AddScoped<ItransactionmasterService, transactionmasterService>();
            services.AddScoped<IusermasterService, usermasterService>();
            services.AddScoped<IuserrolemasterService, userrolemasterService>();
            services.AddScoped<ItokenService, tokenService>();

            services.AddScoped<IboreportService, boreportService>();
            services.AddDbContext<boreportContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));

            services.AddScoped<IboreportdetailService, boreportdetailService>();
            services.AddDbContext<boreportdetailContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));

            services.AddScoped<IboreportcolumnService, boreportcolumnService>();
            services.AddDbContext<boreportcolumnContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddScoped<IboreportothertableService, boreportothertableService>();
            services.AddDbContext<boreportothertableContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));

            services.AddScoped<IboconfigvalueService, boconfigvalueService>();
            services.AddDbContext<boconfigvalueContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));

            services.AddScoped<IbodashboardService, bodashboardService>();
            services.AddDbContext<bodashboardContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));

            /*
            
        private readonly nTireBO.Services.IbodashboardService _bodashboardService;
        private readonly nTireBO.Services.IboreportdetailService _boreportdetailService;
        private readonly nTireBO.Services.IboreportothertableService _boreportothertableService;
        private readonly nTireBO.Services.IboreportcolumnService _boreportcolumnService;
            */

            services.AddDbContext<avatarmasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<carditchargesdiscountContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<citymasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customerdetailContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customermasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customerpaymodeContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customerrecipientlinkContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customersecurityquestionContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customersecurityquestionshistoryContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<customertermsacceptanceContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<geoaccessContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<geographymasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<initiatorrecipientmappingContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<initiatorrecipientprivateContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<masterdataContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<masterdatatypeContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<menuaccessContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<menumasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<recipientdiscountContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<termsmasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<transactiondetailContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<transactionitemdetailContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<transactionmasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<usermasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            services.AddDbContext<userrolemasterContext>(options => options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));

            services.AddMvc().AddNewtonsoftJson();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CardItNow", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CardItNow v1"));
            }
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
