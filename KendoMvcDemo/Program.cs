using Kendo.Mvc;
using KendoMvcDemo.Core.Documents;
using KendoMvcDemo.Core.Models.Student;
using KendoMvcDemo.Infrastructure.Database;
using KendoMvcDemo.Infrastructure.Documents;
using KendoMvcDemo.Infrastructure.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Reflection;
using Telerik.Documents.Core.Fonts;
using Telerik.Reporting.Cache.File;
using Telerik.Reporting.Services;
using Telerik.Reporting.Services.AspNetCore;
using Telerik.Windows.Documents.Fixed.Model.Fonts;

namespace KendoMvcDemo
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<KaustDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Kaust"));
            });

            string reportsPath = Path.Combine(builder.Environment.ContentRootPath, "Reports");

            builder.Services
                .AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    // System.Text.Json
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                })
                .AddNewtonsoftJson(options =>
                {
                    // Newtonsoft.Json
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                })
                .AddTelerikReporting("KaustMvcCore", reportsPath);

            // Telerik Reporting
            builder.Services.AddSingleton<IReportServiceConfiguration>(sp =>
                new ReportServiceConfiguration
                {
                    HostAppId = "KaustMvcCore",
                    Storage = new FileStorage(),
                    ReportSourceResolver = new UriReportSourceResolver(reportsPath),
                    ReportingEngineConfiguration = sp.GetService<IConfiguration>()
                });

            builder.Services.AddScoped<IPdfDocumentGenerator, DefaultPdfDocumentGenerator>();
            builder.Services.AddScoped<IPdfDocumentGenerator<StudentExportDto>, StudentsPdfDocumentGenerator>();
            builder.Services.AddScoped<IExcelDocumentGenerator<StudentExportDto>, StudentsExcelDocumentGenerator>();
            builder.Services.AddScoped<IWordDocumentGenerator<StudentExportDto>, StudentsWordDocumentGenerator>();

            builder.Services.AddCors();

            builder.Services.AddScoped<DataSeedService>();

            builder.Services.AddKendo();

            /* Global deferring */
            //builder.Services.AddKendo(options =>
            //{
            //    options.DeferToScriptFiles = true;
            //});

            // Document processing library fonts
            Telerik.Windows.Documents.Extensibility.FixedExtensibilityManager.FontsProvider = new FontsProvider();

            using (Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("KendoMvcDemo.Fonts.Qahiri-Regular.ttf"))
            {
                using MemoryStream ms = new();
                fontStream.CopyTo(ms);
                FontsRepository.RegisterFont(new FontFamily("Qahiri"), FontStyles.Normal, FontWeights.Normal, ms.ToArray());
            }

            if (builder.Environment.IsDevelopment())
            {
                builder.Services
                    .AddRazorPages()
                    .AddRazorRuntimeCompilation();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseTelerikReporting();
            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<KendoDeferredScriptsMiddleware>();

            app.MapStaticAssets();

            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.MapControllers();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            string cultureName = "en-US";   // ar-AE
            CultureInfo[] supportedCultures = [new CultureInfo(cultureName)];

            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(cultureName),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            CultureInfo.CurrentCulture = new CultureInfo(cultureName);
            CultureInfo.CurrentUICulture = new CultureInfo(cultureName);

            CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";

            using (IServiceScope scope = app.Services.CreateScope())
            {
                KaustDbContext db = scope.ServiceProvider.GetRequiredService<KaustDbContext>();
                await db.Database.MigrateAsync();

                DataSeedService dataSeedService = scope.ServiceProvider.GetRequiredService<DataSeedService>();
                await dataSeedService.SeedAsync();
            }

            await app.RunAsync();
        }
    }
}
